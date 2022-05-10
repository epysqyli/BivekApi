using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Api.Models;

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
            IEnumerable<Article> articles = _unitOfWork.Articles.GetAll();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            Article article = _unitOfWork.Articles.GetById(id);
            return (article == null) ? NotFound() : Ok(article);
        }

        [HttpPost]
        public IActionResult CreateArticle(Article article)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Articles.Add(article);
                _unitOfWork.Complete();
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
        public IActionResult DeleteArticle(int id)
        {
            Article existingArticle = _unitOfWork.Articles.GetById(id);
            if (existingArticle == null)
                return NotFound();

            _unitOfWork.Articles.Remove(existingArticle);
            _unitOfWork.Complete();

            return Ok(existingArticle);
        }
    }
}