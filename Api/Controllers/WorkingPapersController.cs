using Api.Models;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkingPapersController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public WorkingPapersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetWorkingPapers()
        {
            IEnumerable<IWorkingPaperDto> workingPaperDtos = _unitOfWork.WorkingPapers.GetAllDtos();
            return Ok(workingPaperDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetWorkingPaper(int id)
        {
            IWorkingPaperDto workingPaperDto = _unitOfWork.WorkingPapers.GetDto(id);
            return (workingPaperDto == null) ? NotFound() : Ok(workingPaperDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateWorkingPaper(WorkingPaper workingPaper)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.WorkingPapers.AddAsync(workingPaper);
                await _unitOfWork.CompleteAsync();
                return CreatedAtAction("CreateWorkingPaper", new { workingPaper.Id }, workingPaper);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateWorkingPaper(int id, JsonPatchDocument<WorkingPaper> workingPaperPatch)
        {
            WorkingPaper workingPaper = _unitOfWork.WorkingPapers.GetById(id);
            if (workingPaper == null)
                return NotFound();

            _unitOfWork.WorkingPapers.Patch(workingPaper, workingPaperPatch);
            WorkingPaper updatedWorkingPaper = _unitOfWork.WorkingPapers.GetById(id);
            _unitOfWork.Complete();

            return CreatedAtAction("UpdateWorkingPaper", new { workingPaper.Id }, workingPaper);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteWorkingPaper(int id)
        {
            WorkingPaper existingWorkingPaper = _unitOfWork.WorkingPapers.GetById(id);
            if (existingWorkingPaper == null)
                return NotFound();

            _unitOfWork.WorkingPapers.Remove(existingWorkingPaper);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
