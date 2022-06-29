using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using System.Collections.Generic;

using Api.Models.Entities;
using Api.Models.Dtos;
using Api.Interfaces;
using Api.Controllers;

namespace Api.UnitTests.Controllers
{
    public class DataCategoriestControllerTest
    {
        [Fact]
        public void GetDataCategory_Returns_NotFound_GivenNoDataCategory()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            int dataCategoryId = 1;
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetDto(dataCategoryId)).Returns<IDataCategoryDto>(null);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult res = dataCategoriesController.GetDataCategory(dataCategoryId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void GetDataCategory_Returns_DataCategoryDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IDataCategoryDto> dataCategoryDto = new Mock<IDataCategoryDto>();
            DataCategory dataCategory = new DataCategory() { Id = 1, Name = "Random Data Category", Datasets = GetDatasets() };
            SetupDataCategoryDto(dataCategoryDto, dataCategory.Id, dataCategory.Name, GetDatasetDtos());
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetDto(dataCategory.Id)).Returns(dataCategoryDto.Object);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult response = dataCategoriesController.GetDataCategory(dataCategory.Id);
            OkObjectResult result = (OkObjectResult)response;

            Assert.IsType<OkObjectResult>(response);
            if (result.Value != null)
            {
                IDataCategoryDto dtoResult = (IDataCategoryDto)result.Value;
                Assert.Equal(dataCategory.Id, dtoResult.Id);
                Assert.Equal(dataCategory.Name, dtoResult.Name);
            }
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);
            dataCategoriesController.ModelState.AddModelError("error", "generic error");

            DataCategory dataCategory = new DataCategory() { Name = null };
            IActionResult res = await dataCategoriesController.CreateDataCategory(dataCategory);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async Task Create_Returns_DateCategoryDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            DataCategory dataCategory = new DataCategory() { Id = 1, Name = "Random Data Category", Datasets = GetDatasets() };
            Moq.Mock<IDataCategoryDto> dataCategoryDto = new Mock<IDataCategoryDto>();
            SetupDataCategoryDto(dataCategoryDto, dataCategory.Id, dataCategory.Name, GetDatasetDtos());
            mockIUnitOfWork.Setup(unit => unit.DataCategories.Add(dataCategory));
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetDto(dataCategory.Id)).Returns(dataCategoryDto.Object);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult response = await dataCategoriesController.CreateDataCategory(dataCategory);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                IDataCategoryDto dtoResult = (IDataCategoryDto)result.Value;
                Assert.Equal(dataCategory.Id, dtoResult.Id);
                Assert.Equal(dataCategory.Name, dtoResult.Name);
            }
        }

        [Fact]
        public void Update_Returns_EditedDataCategoryDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            DataCategory dataCategory = new DataCategory() { Id = 1, Name = "Random Data Category", Datasets = GetDatasets() };
            string namePatch = "Edited Category Name";
            JsonPatchDocument<DataCategory> dataCategoryPatch = new JsonPatchDocument<DataCategory>().Replace(dc => dc.Name, namePatch);
            Moq.Mock<IDataCategoryDto> dataCategoryDto = new Mock<IDataCategoryDto>();
            SetupDataCategoryDto(dataCategoryDto, dataCategory.Id, namePatch, GetDatasetDtos());
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetById(dataCategory.Id)).Returns(dataCategory);
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetDto(dataCategory.Id)).Returns(dataCategoryDto.Object);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult response = dataCategoriesController.UpdateDataCategory(dataCategory.Id, dataCategoryPatch);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                IDataCategoryDto dtoResult = (IDataCategoryDto)result.Value;
                Assert.Equal(dataCategory.Id, dtoResult.Id);
                Assert.Equal(dtoResult.Name, namePatch);
            }
        }

        [Fact]
        public async Task Delete_Returns_NoContent()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            DataCategory dataCategory = new DataCategory() { Id = 1, Name = "Random Data Category" };
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetById(dataCategory.Id)).Returns(dataCategory);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult res = await dataCategoriesController.DeleteDataCategory(dataCategory.Id);

            Assert.IsType<NoContentResult>(res);
        }

        private void SetupDataCategoryDto(Mock<IDataCategoryDto> dataCategoryDto, int Id, string Name, IEnumerable<IDatasetDto> datasetDtos)
        {
            dataCategoryDto.SetupAllProperties();
            dataCategoryDto.Object.Id = Id;
            dataCategoryDto.Object.Name = Name;
            dataCategoryDto.Object.Datasets = datasetDtos;
        }

        private ICollection<Dataset> GetDatasets()
        {
            Dataset first = new Dataset() { Id = 1, Title = "First Dataset", Link = "First Link", DataCategoryId = 1 };
            Dataset second = new Dataset() { Id = 2, Title = "Second Dataset", Link = "Second Link", DataCategoryId = 1 };
            return new List<Dataset>() { first, second };
        }

        private IEnumerable<IDatasetDto> GetDatasetDtos()
        {
            DatasetDto firstDto = new DatasetDto() { Id = 1, Title = "First Dataset", Link = "First Link", DataCategoryId = 1 };
            DatasetDto secondTo = new DatasetDto() { Id = 2, Title = "Second Dataset", Link = "Second Link", DataCategoryId = 1 };
            return new List<DatasetDto>() { firstDto, secondTo };
        }
    }
}