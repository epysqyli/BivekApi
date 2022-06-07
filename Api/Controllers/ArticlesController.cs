using Api.Models;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Api.Controllers
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public ArticlesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetArticles()
        {
            IEnumerable<IArticleDto> articleDtos = _unitOfWork.Articles.GetAllDtos();
            return Ok(articleDtos);
        }

        [HttpGet("published")]
        public IActionResult GetPublishedArticles()
        {
            IEnumerable<IArticleDto> articleDtos = _unitOfWork.Articles.GetAllPublishedDtos();
            return Ok(articleDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            IArticleDto article = _unitOfWork.Articles.GetDto(id);
            return (article.isNull()) ? NotFound() : Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Articles.AddAsync(article);
                await _unitOfWork.CompleteAsync();
                return CreatedAtAction("CreateArticle", new { article.Id }, article);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateArticle(int id, JsonPatchDocument<Article> articlePatch)
        {
            Article article = _unitOfWork.Articles.GetById(id);
            if (article == null)
                return NotFound();

            _unitOfWork.Articles.Patch(article, articlePatch);
            Article updatedArticle = _unitOfWork.Articles.GetById(id);
            _unitOfWork.Complete();

            return CreatedAtAction("UpdateArticle", new { article.Id }, article);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            Article existingArticle = _unitOfWork.Articles.GetById(id);
            if (existingArticle == null)
                return NotFound();

            _unitOfWork.Articles.Remove(existingArticle);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
