using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

using Api.Models.Entities;
using Api.Interfaces;
using Api.Controllers;

namespace Api.UnitTests.Controllers
{
    public class DatasetControllerTest
    {
        [Fact]
        public void GetDataset_Returns_NotFound_GivenNoDataset()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            int datasetId = 1;
            mockIUnitOfWork.Setup(unit => unit.Datasets.GetDto(datasetId)).Returns<IDatasetDto>(null);
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);

            IActionResult response = datasetsController.GetDataset(datasetId);

            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void GetDataset_Returns_DatasetDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IDatasetDto> datasetDto = new Mock<IDatasetDto>();
            Dataset dataset = new Dataset() { Id = 1, DataCategoryId = 1, Link = "Some link", Title = "Some title" };
            SetupDatasetDto(datasetDto, dataset.Id, dataset.Title, dataset.Link, dataset.DataCategoryId);
            mockIUnitOfWork.Setup(unit => unit.Datasets.GetDto(dataset.Id)).Returns(datasetDto.Object);
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);

            IActionResult response = datasetsController.GetDataset(dataset.Id);
            OkObjectResult okResult = (OkObjectResult)response;

            Assert.IsType<OkObjectResult>(response);
            if (okResult.Value != null)
            {
                IDatasetDto dtoResult = (IDatasetDto)okResult.Value;
                Assert.Equal(dtoResult.Id, dataset.Id);
                Assert.Equal(dtoResult.Title, dataset.Title);
                Assert.Equal(dtoResult.Link, dataset.Link);
            }
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);
            datasetsController.ModelState.AddModelError("error", "generic error");

            Dataset dataset = new Dataset() { Title = null };
            IActionResult response = await datasetsController.CreateDataset(dataset);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task Create_Returns_DatasetDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Dataset dataset = new Dataset() { Title = "Test Title", Link = "Some link", DataCategoryId = 1 };
            Moq.Mock<IDatasetDto> datasetDto = new Mock<IDatasetDto>();
            SetupDatasetDto(datasetDto, dataset.Id, dataset.Title, dataset.Link, dataset.DataCategoryId);
            mockIUnitOfWork.Setup(unit => unit.Datasets.Add(dataset));
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);

            IActionResult response = await datasetsController.CreateDataset(dataset);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                IDatasetDto dtoResult = (IDatasetDto)result.Value;
                Assert.Equal(dataset.Id, dtoResult.Id);
                Assert.Equal(dataset.Title, dtoResult.Title);
                Assert.Equal(dataset.Link, dtoResult.Link);
                Assert.Equal(dataset.DataCategoryId, dtoResult.DataCategoryId);
            }
        }


        [Fact]
        public void Update_Returns_EditedDatasetDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Dataset dataset = new Dataset() { Id = 1, Title = "Test Title", Link = "Some link", DataCategoryId = 1 };
            string titlePatch = "Edited Title";
            JsonPatchDocument<Dataset> datasetPatch = new JsonPatchDocument<Dataset>().Replace(a => a.Title, titlePatch);
            Moq.Mock<IDatasetDto> datasetDto = new Mock<IDatasetDto>();
            SetupDatasetDto(datasetDto, dataset.Id, titlePatch, dataset.Link, dataset.DataCategoryId);
            mockIUnitOfWork.Setup(unit => unit.Datasets.GetById(dataset.Id)).Returns(dataset);
            mockIUnitOfWork.Setup(unit => unit.Datasets.GetDto(dataset.Id)).Returns(datasetDto.Object);
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);

            IActionResult response = datasetsController.UpdateDataset(dataset.Id, datasetPatch);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                IDatasetDto dtoResult = (IDatasetDto)result.Value;
                Assert.Equal(dataset.Id, dtoResult.Id);
                Assert.Equal(titlePatch, dtoResult.Title);
            }
        }

        [Fact]
        public async Task Delete_Returns_NoContent()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Dataset dataset = new Dataset() { Title = "Test Title", Link = "Some link", DataCategoryId = 1 };
            mockIUnitOfWork.Setup(unit => unit.Datasets.GetById(dataset.Id)).Returns(dataset);
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);

            IActionResult res = await datasetsController.DeleteDataset(dataset.Id);

            Assert.IsType<NoContentResult>(res);
        }

        private void SetupDatasetDto(Mock<IDatasetDto> datasetDto, int Id, string Title, string Link, int DataCategoryId)
        {
            datasetDto.SetupAllProperties();
            datasetDto.Object.Id = Id;
            datasetDto.Object.Title = Title;
            datasetDto.Object.Link = Link;
            datasetDto.Object.DataCategoryId = DataCategoryId;
        }
    }
}