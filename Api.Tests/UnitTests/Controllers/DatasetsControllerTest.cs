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

            IActionResult res = datasetsController.GetDataset(datasetId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void GetDataset_Returns_OkObjectResult()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IDatasetDto> datasetDto = new Mock<IDatasetDto>();
            Dataset dataset = new Dataset() { Id = 1, DataCategoryId = 1, Link = "Some link", Title = "Some title" };
            mockIUnitOfWork.Setup(unit => unit.Datasets.GetDto(dataset.Id)).Returns(datasetDto.Object);
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);

            IActionResult res = datasetsController.GetDataset(dataset.Id);

            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);
            datasetsController.ModelState.AddModelError("error", "generic error");

            Dataset dataset = new Dataset() { Title = null };
            IActionResult res = await datasetsController.CreateDataset(dataset);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async Task Create_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Dataset dataset = new Dataset() { Title = "Test Title", Link = "Some link", DataCategoryId = 1 };
            mockIUnitOfWork.Setup(unit => unit.Datasets.Add(dataset));
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);

            IActionResult res = await datasetsController.CreateDataset(dataset);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void Update_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Dataset dataset = new Dataset() { Id = 1, Title = "Test Title", Link = "Some link", DataCategoryId = 1 };
            mockIUnitOfWork.Setup(unit => unit.Datasets.GetById(dataset.Id)).Returns(dataset);
            DatasetsController datasetsController = new DatasetsController(mockIUnitOfWork.Object);
            JsonPatchDocument<Dataset> datasetPatch = new JsonPatchDocument<Dataset>();
            datasetPatch.Replace(a => a.Title, "New Title");

            IActionResult res = datasetsController.UpdateDataset(1, datasetPatch);

            Assert.IsType<CreatedAtActionResult>(res);
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
    }
}