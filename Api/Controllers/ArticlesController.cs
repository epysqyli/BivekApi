using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Api.Data;
using Api.Models;

namespace Api.Controllers
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public ArticlesController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            List<int> ids = await _context.Articles.Select(a => a.Id).ToListAsync();
            List<ArticleDto> articles = new List<ArticleDto>();
            foreach (int id in ids)
                articles.Add(await ArticleDto.Create(id, _context));
            
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            ArticleDto article = await ArticleDto.Create(id, _context);
            return (article == null) ? NotFound() : Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();

                return CreatedAtAction("CreateArticle", new { article.Id }, article);
            }

            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, Article article)
        {
            if (id != article.Id)
                return BadRequest();

            Article existingArticle = await _context.Articles.FindAsync(id);
            if (existingArticle == null)
                return NotFound();

            existingArticle.Title = article.Title;
            existingArticle.Body = article.Body;

            await _context.SaveChangesAsync();
            return CreatedAtAction("UpdateArticle", new { existingArticle.Id }, existingArticle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            Article existingArtile = await _context.Articles.FindAsync(id);
            if (existingArtile == null)
                return NotFound();

            _context.Articles.Remove(existingArtile);
            await _context.SaveChangesAsync();

            return Ok(existingArtile);
        }
    }
}