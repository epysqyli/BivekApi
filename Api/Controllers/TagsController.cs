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
            IEnumerable<ITagDto> tagDtos = _unitOfWork.Tags.GetAllDtos();
            return Ok(tagDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetTag(int id)
        {
            ITagDto tagDto = _unitOfWork.Tags.GetDto(id);
            return (tagDto == null) ? NotFound() : Ok(tagDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Tags.AddAsync(tag);
                await _unitOfWork.CompleteAsync();
                ITagDto tagDto = _unitOfWork.Tags.GetDto(tag.Id);
                return CreatedAtAction("CreateTag", new { tag.Id }, tagDto);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateTag(int id, JsonPatchDocument<Tag> tagPatch)
        {
            Tag tag = _unitOfWork.Tags.GetById(id);
            if (tag == null)
                return NotFound();

            _unitOfWork.Tags.Patch(tag, tagPatch);
            _unitOfWork.Complete();
            ITagDto tagDto = _unitOfWork.Tags.GetDto(tag.Id);

            return CreatedAtAction("UpdateTag", new { tag.Id }, tagDto);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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