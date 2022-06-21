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
    public class DatasetsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public DatasetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetDatasets()
        {
            IEnumerable<IDatasetDto> datasets = _unitOfWork.Datasets.GetAllDtos();
            return Ok(datasets);
        }

        [HttpGet("{id}")]
        public IActionResult GetDataset(int id)
        {
            IDatasetDto datasetDto = _unitOfWork.Datasets.GetDto(id);
            return (datasetDto == null) ? NotFound() : Ok(datasetDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateDataset(Dataset dataset)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Datasets.AddAsync(dataset);
                await _unitOfWork.CompleteAsync();
                return CreatedAtAction("CreateDataset", new { dataset.Id }, dataset);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateDataset(int id, JsonPatchDocument<Dataset> datasetPatch)
        {
            Dataset dataset = _unitOfWork.Datasets.GetById(id);
            if (dataset == null)
                return NotFound();

            _unitOfWork.Datasets.Patch(dataset, datasetPatch);
            Dataset updatedDataset = _unitOfWork.Datasets.GetById(id);
            _unitOfWork.Complete();

            return CreatedAtAction("UpdateDataset", new { dataset.Id }, dataset);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteDataset(int id)
        {
            Dataset existingDataset = _unitOfWork.Datasets.GetById(id);
            if (existingDataset == null)
                return NotFound();

            _unitOfWork.Datasets.Remove(existingDataset);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
