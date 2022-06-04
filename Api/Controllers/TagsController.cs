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
    public class TagsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public TagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetTags()
        {
            IEnumerable<Tag> tags = _unitOfWork.Tags.GetAll();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public IActionResult GetTag(int id)
        {
            Tag tag = _unitOfWork.Tags.GetById(id);
            return (tag == null) ? NotFound() : Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Tags.AddAsync(tag);
                await _unitOfWork.CompleteAsync();
                return CreatedAtAction("CreateTag", new { tag.Id }, tag);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateTag(int id, JsonPatchDocument<Tag> tagPatch)
        {
            Tag tag = _unitOfWork.Tags.GetById(id);
            if (tag == null)
                return NotFound();

            _unitOfWork.Tags.Patch(tag, tagPatch);
            Article updatedArticle = _unitOfWork.Articles.GetById(id);
            _unitOfWork.Complete();

            return CreatedAtAction("UpdateTag", new { tag.Id }, tag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            Tag existingTag = _unitOfWork.Tags.GetById(id);
            if (existingTag == null)
                return NotFound();

            _unitOfWork.Tags.Remove(existingTag);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}