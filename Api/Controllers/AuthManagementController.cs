using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Api.Models;
using Api.Configuration;

namespace Api
{
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Result = false,
                        Errors = new List<string>() { "Email already exist" }
                    });
                }

                IdentityUser newUser = new IdentityUser() { Email = user.Email, UserName = user.Email };
                IdentityResult isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    string jwtToken = GenerateJwtToken(newUser);

                    return Ok(new RegistrationResponse()
                    {
                        Result = true,
                        Token = jwtToken
                    });
                }

                return new JsonResult(new RegistrationResponse()
                {
                    Result = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
                })
                { StatusCode = 500 };
            }

            return BadRequest(new RegistrationResponse()
            {
                Result = false,
                Errors = new List<string>() { "Invalid payload" }
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser existingUser = await _userManager.FindByNameAsync(user.Username);

                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Result = false,
                        Errors = new List<string>() { "Invalid authentication request" }
                    });
                }

                bool isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (isCorrect)
                {
                    string jwtToken = GenerateJwtToken(existingUser);

                    return Ok(new RegistrationResponse()
                    {
                        Result = true,
                        Token = jwtToken,
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Result = false,
                        Errors = new List<string>() { "Invalid authentication request" }
                    });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Result = false,
                Errors = new List<string>() { "Invalid payload" }
            });
        }

        private string GenerateJwtToken(IdentityUser user)
        {

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(6),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}