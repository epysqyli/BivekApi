using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Api.Data;
using Api.Models;

namespace Api.Controllers
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private IArticleRepository _articleRepo;
        public ArticlesController(ApiDbContext context)
        {
            _articleRepo = new ArticleRepo(context);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            IEnumerable<ArticleDto> articles = await _articleRepo.GetArticleDtos();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            ArticleDto article = await _articleRepo.GetArticleDtoById(id);
            return (article == null) ? NotFound() : Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                await _articleRepo.InsertArticle(article);
                await _articleRepo.Save();
                return CreatedAtAction("CreateArticle", new { article.Id }, article);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, JsonPatchDocument articlePatch)
        {
            Article article = await _articleRepo.GetArticleById(id);
            if (article == null)
                return NotFound();

            _articleRepo.PartialUpdateArticle(article, articlePatch);
            ArticleDto updatedArticle = await _articleRepo.GetArticleDtoById(id);
            return CreatedAtAction("UpdateArticle", new { article.Id }, article);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            Article existingArticle = await _articleRepo.GetArticleById(id);
            if (existingArticle == null)
                return NotFound();

            await _articleRepo.DeleteArticle(id);
            await _articleRepo.Save();

            return Ok(existingArticle);
        }
    }
}