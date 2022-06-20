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
    public class WorkingPapersControllerTest
    {
        [Fact]
        public void GetTag_Returns_NotFound_GivenNoTag()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            int workingPaperId = 1;
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetDto(workingPaperId)).Returns<IWorkingPaperDto>(null);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult res = workingPapersController.GetWorkingPaper(workingPaperId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void GetTag_Returns_OkObjectResult()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IWorkingPaperDto> workingPaperDto = new Mock<IWorkingPaperDto>();
            WorkingPaper workingPaper = new WorkingPaper() { Id = 1, Title = "Random Title", Link = "Some link", Abstract = "Some abstract" };
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetDto(workingPaper.Id)).Returns(workingPaperDto.Object);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult res = workingPapersController.GetWorkingPaper(workingPaper.Id);

            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);
            workingPapersController.ModelState.AddModelError("error", "generic error");

            WorkingPaper workingPaper = new WorkingPaper() { Title = null };
            IActionResult res = await workingPapersController.CreateWorkingPaper(workingPaper);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async Task Create_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            WorkingPaper workingPaper = new WorkingPaper() { Id = 1, Title = "Random Title", Link = "Some link", Abstract = "Some abstract" };
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.Add(workingPaper));
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult res = await workingPapersController.CreateWorkingPaper(workingPaper);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void Update_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            WorkingPaper workingPaper = new WorkingPaper() { Id = 1, Title = "Random Title", Link = "Some link", Abstract = "Some abstract" };
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetById(workingPaper.Id)).Returns(workingPaper);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);
            JsonPatchDocument<WorkingPaper> workingPaperPatch = new JsonPatchDocument<WorkingPaper>();
            workingPaperPatch.Replace(wp => wp.Title, "New Title");

            IActionResult res = workingPapersController.UpdateWorkingPaper(1, workingPaperPatch);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public async Task Delete_Returns_NoContent()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            WorkingPaper workingPaper = new WorkingPaper() { Id = 1, Title = "Random Title", Link = "Some link", Abstract = "Some abstract" };
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetById(workingPaper.Id)).Returns(workingPaper);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult res = await workingPapersController.DeleteWorkingPaper(workingPaper.Id);

            Assert.IsType<NoContentResult>(res);
        }
    }
}