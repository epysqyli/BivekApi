using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
        public void GetDataCategories_Returns_AllDataCategoryDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IDataCategoryDto> firstDataCategoryDto = new Mock<IDataCategoryDto>();
            Moq.Mock<IDataCategoryDto> secondDataCategoryDto = new Mock<IDataCategoryDto>();
            DataCategory firstDataCategory = new DataCategory() { Id = 1, Name = "First Data Category", Datasets = GetDatasets() };
            DataCategory secondDataCategory = new DataCategory() { Id = 2, Name = "Second Data Category", Datasets = GetMoreDatasets() };
            SetupDataCategoryDto(firstDataCategoryDto, firstDataCategory.Id, firstDataCategory.Name, GetDatasetDtos());
            SetupDataCategoryDto(secondDataCategoryDto, secondDataCategory.Id, secondDataCategory.Name, GetMoreDatasetDtos());
            IEnumerable<IDataCategoryDto> dtos = new List<IDataCategoryDto>() { firstDataCategoryDto.Object, secondDataCategoryDto.Object };
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetAllDtos()).Returns(dtos);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult response = dataCategoriesController.GetDataCategories();
            OkObjectResult result = (OkObjectResult)response;

            Assert.IsType<OkObjectResult>(response);
            if (result.Value != null)
            {
                IEnumerable<IDataCategoryDto> dtoResult = (IEnumerable<IDataCategoryDto>)result.Value;
                Assert.Equal(dtos.Count(), dtoResult.Count());
            }
        }

        [Fact]
        public void GetNonEmptyDataCategories_Returns_NonEmptyDataCategories()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IDataCategoryDto> firstDataCategoryDto = new Mock<IDataCategoryDto>();
            Moq.Mock<IDataCategoryDto> secondDataCategoryDto = new Mock<IDataCategoryDto>();
            DataCategory firstDataCategory = new DataCategory() { Id = 1, Name = "First Data Category", Datasets = GetDatasets() };
            DataCategory secondDataCategory = new DataCategory() { Id = 2, Name = "Second Data Category", Datasets = GetEmptyDatasets() };
            SetupDataCategoryDto(firstDataCategoryDto, firstDataCategory.Id, firstDataCategory.Name, GetDatasetDtos());
            SetupDataCategoryDto(secondDataCategoryDto, secondDataCategory.Id, secondDataCategory.Name, GetEmptyDatasetDtos());
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetNonEmptyDtos())
                                                              .Returns(new List<IDataCategoryDto>() { firstDataCategoryDto.Object });
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult response = dataCategoriesController.GetNonEmptyDataCategories();
            OkObjectResult result = (OkObjectResult)response;

            Assert.IsType<OkObjectResult>(response);
            if (result.Value != null)
            {
                IEnumerable<IDataCategoryDto> dtoResult = (IEnumerable<IDataCategoryDto>)result.Value;
                Assert.Equal(1, dtoResult.Count());
            }
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
                Assert.Equal(dtoResult.Id, dataCategory.Id);
                Assert.Equal(dtoResult.Name, dataCategory.Name);
                Assert.Equal(dataCategory.Datasets.Count, dtoResult.Datasets.Count());
                Assert.Equal(dataCategory.Datasets.ElementAt(0).Id, dtoResult.Datasets.ElementAt(0).Id);
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
                Assert.Equal(dtoResult.Id, dataCategory.Id);
                Assert.Equal(dtoResult.Name, dataCategory.Name);
                Assert.Equal(dataCategory.Datasets.Count, dtoResult.Datasets.Count());
                Assert.Equal(dataCategory.Datasets.ElementAt(0).Id, dtoResult.Datasets.ElementAt(0).Id);
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
            return new List<Dataset>()
            {
                new Dataset() { Id = 1, Title = "First Dataset", Link = "First Link", DataCategoryId = 1 },
                new Dataset() { Id = 2, Title = "Second Dataset", Link = "Second Link", DataCategoryId = 1 }
            };
        }

        private ICollection<Dataset> GetMoreDatasets()
        {
            return new List<Dataset>()
            {
                new Dataset() { Id = 3, Title = "Third Dataset", Link = "Third Link", DataCategoryId = 2 },
                new Dataset() { Id = 4, Title = "Fourth Dataset", Link = "Fourth Link", DataCategoryId = 2 }
            };
        }

        private IEnumerable<IDatasetDto> GetDatasetDtos()
        {
            return new List<DatasetDto>()
            {
                new DatasetDto() { Id = 1, Title = "First Dataset", Link = "First Link", DataCategoryId = 1 },
                new DatasetDto() { Id = 2, Title = "Second Dataset", Link = "Second Link", DataCategoryId = 1 }
            };
        }

        private IEnumerable<IDatasetDto> GetMoreDatasetDtos()
        {
            return new List<DatasetDto>()
            {
                new DatasetDto() { Id = 3, Title = "Third Dataset", Link = "Third Link", DataCategoryId = 2 },
                new DatasetDto() { Id = 4, Title = "Fourth Dataset", Link = "Fourth Link", DataCategoryId = 2 }
            };
        }

        private ICollection<Dataset> GetEmptyDatasets()
        {
            return new List<Dataset>() { };
        }

        private IEnumerable<IDatasetDto> GetEmptyDatasetDtos()
        {
            return new List<DatasetDto>() { };
        }
    }
}