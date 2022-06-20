using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

using Api.Models;
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
        public void GetDataCategory_Returns_OkObjectResult()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IDataCategoryDto> dataCategoryDto = new Mock<IDataCategoryDto>();
            DataCategory dataCategory = new DataCategory() { Id = 1, Name = "Random Data Category" };
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetDto(dataCategory.Id)).Returns(dataCategoryDto.Object);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult res = dataCategoriesController.GetDataCategory(dataCategory.Id);

            Assert.IsType<OkObjectResult>(res);
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
        public async Task Create_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            DataCategory dataCategory = new DataCategory() { Id = 1, Name = "Random Data Category" };
            mockIUnitOfWork.Setup(unit => unit.DataCategories.Add(dataCategory));
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);

            IActionResult res = await dataCategoriesController.CreateDataCategory(dataCategory);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void Update_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            DataCategory dataCategory = new DataCategory() { Id = 1, Name = "Random Data Category" };
            mockIUnitOfWork.Setup(unit => unit.DataCategories.GetById(dataCategory.Id)).Returns(dataCategory);
            DataCategoriesController dataCategoriesController = new DataCategoriesController(mockIUnitOfWork.Object);
            JsonPatchDocument<DataCategory> dataCategoryPatch = new JsonPatchDocument<DataCategory>();
            dataCategoryPatch.Replace(dc => dc.Name, "New Data Category Name");

            IActionResult res = dataCategoriesController.UpdateDataCategory(1, dataCategoryPatch);

            Assert.IsType<CreatedAtActionResult>(res);
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
    }
}