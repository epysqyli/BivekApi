using Api.Models;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticleTagsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public ArticleTagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public IActionResult GetArticlesByTagId(int tagId)
        {
            IEnumerable<IArticleDto> articleDtos = _unitOfWork.Articles.GetArticlesByTagId(tagId);
            return Ok(articleDtos);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateArticleTagRelation(ArticleTag articleTag)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Articles.AddTagToArticle(articleTag);
                return CreatedAtAction("CreateArticleTagRelation", new { articleTag }, articleTag);
            }

            return BadRequest("Something went wrong");
        }

        [HttpDelete("{articleId}-{tagId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteArticleTagRelation(int articleId, int tagId)
        {   
            await _unitOfWork.Articles.RemoveTagFromArticle(articleId, tagId);
            return NoContent();
        }
    }
}