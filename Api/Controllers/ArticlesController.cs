using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;

namespace Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, JsonPatchDocument articlePatch)
        {
            Article article = await _context.Articles.FindAsync(id);
            await article.PatchArticle(id, articlePatch, _context);

            return CreatedAtAction("CreateArticle", new { article.Id }, article);
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