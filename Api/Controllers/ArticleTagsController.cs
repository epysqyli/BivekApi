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
    public class ArticleTagsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public ArticleTagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public IActionResult GetArticlesByTagId(int id)
        {
            IEnumerable<IArticleDto> articleDtos = _unitOfWork.ArticleTags.Find(at => at.TagId == id)
                                                                          .ToList()
                                                                          .Select(t => _unitOfWork.Articles.GetDto(t.ArticleId));

            return Ok(articleDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticleTagRelation(ArticleTag articleTag)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.ArticleTags.AddAsync(articleTag);
                await _unitOfWork.CompleteAsync();
                return CreatedAtAction("CreateArticleTagRelation", new { articleTag }, articleTag);
            }

            return BadRequest("Something went wrong");
        }

        [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteArticleTagRelation(ArticleTag articleTag)
        // {
        //     ArticleTag existingArticleTag = _unitOfWork.ArticleTags.GetById(id);
        //     if (existingArticleTag == null)
        //         return NotFound();

        //     _unitOfWork.ArticleTags.Remove(existingArticleTag);
        //     await _unitOfWork.CompleteAsync();

        //     return NoContent();
        // }
    }
}