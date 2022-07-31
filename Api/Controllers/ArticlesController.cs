using Api.Models.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Api.Controllers
{
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpGet("latest")]
        public IActionResult GetLatestArticle()
        {
            IArticleDto articleDto = _unitOfWork.Articles.GetAllPublishedDtos().First();
            return Ok(articleDto);
        }

        [HttpGet("tags")]
        public IActionResult GetArticlesByTagId(int[] ids)
        {
            IEnumerable<IArticleDto> articleDtos = _unitOfWork.Articles.GetArticlesByTagIds(ids);
            return Ok(articleDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            IArticleDto article = _unitOfWork.Articles.GetDto(id);
            return (article.isNull()) ? NotFound() : Ok(article);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.CompleteAsync();
            IArticleDto articleDto = _unitOfWork.Articles.GetDto(article.Id);
            return CreatedAtAction("CreateArticle", new { article.Id }, articleDto);
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateArticle(int id, JsonPatchDocument<Article> articlePatch)
        {
            Article article = _unitOfWork.Articles.GetById(id);
            if (article == null) return NotFound();

            string oldTitle = article.Title;
            _unitOfWork.Articles.Patch(article, articlePatch);
            if (oldTitle != article.Title)
                if (!TryValidateModel(article)) return BadRequest(ModelState);

            _unitOfWork.Complete();
            IArticleDto articleDto = _unitOfWork.Articles.GetDto(id);

            return CreatedAtAction("UpdateArticle", new { article.Id }, articleDto);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
