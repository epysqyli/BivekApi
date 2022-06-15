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
    public class DataCategoriesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public DataCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetDataCategories()
        {
            IEnumerable<IDataCategoryDto> dataCategoryDtos = _unitOfWork.DataCategories.GetAllDtos();
            return Ok(dataCategoryDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetDataCategory(int id)
        {
            IDataCategoryDto dataCategoryDto = _unitOfWork.DataCategories.GetDto(id);
            return (dataCategoryDto == null) ? NotFound() : Ok(dataCategoryDto);
        }

        [HttpPost]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateDataCategory(DataCategory dataCategory)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DataCategories.AddAsync(dataCategory);
                await _unitOfWork.CompleteAsync();
                return CreatedAtAction("CreateDataCategory", new { dataCategory.Id }, dataCategory);
            }

            return BadRequest("Something went wrong");
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateDataCategory(int id, JsonPatchDocument<DataCategory> dataCategoryPatch)
        {
            DataCategory dataCategory = _unitOfWork.DataCategories.GetById(id);
            if (dataCategory == null)
                return NotFound();

            _unitOfWork.DataCategories.Patch(dataCategory, dataCategoryPatch);
            _unitOfWork.Complete();
            DataCategory updatedDataCategory = _unitOfWork.DataCategories.GetById(id);

            return CreatedAtAction("UpdateDataCategory", new { dataCategory.Id }, dataCategory);
        }

        [HttpDelete("{id}")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteDataCategory(int id)
        {
            DataCategory existingDataCategory = _unitOfWork.DataCategories.GetById(id);
            if (existingDataCategory == null)
                return NotFound();

            _unitOfWork.DataCategories.Remove(existingDataCategory);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}