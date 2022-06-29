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
    public class WorkingPapersControllerTest
    {
        [Fact]
        public void GetWorkingPaper_Returns_NotFound_GivenNoWorkingPaper()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            int workingPaperId = 1;
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetDto(workingPaperId)).Returns<IWorkingPaperDto>(null);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult res = workingPapersController.GetWorkingPaper(workingPaperId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void GetWorkingPaper_Returns_WorkingPaperDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IWorkingPaperDto> workingPaperDto = new Mock<IWorkingPaperDto>();
            WorkingPaper workingPaper = new WorkingPaper()
            {
                Id = 1,
                Title = "Random Title",
                Link = "Some link",
                Abstract = "Some abstract"
            };
            SetupWorkingPaperDto(workingPaperDto, workingPaper.Id, workingPaper.Title, workingPaper.Link, workingPaper.Abstract);
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetDto(workingPaper.Id)).Returns(workingPaperDto.Object);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult response = workingPapersController.GetWorkingPaper(workingPaper.Id);
            OkObjectResult result = (OkObjectResult)response;

            Assert.IsType<OkObjectResult>(response);
            if (result.Value != null)
            {
                IWorkingPaperDto dtoResult = (IWorkingPaperDto)result.Value;
                Assert.Equal(workingPaper.Id, dtoResult.Id);
                Assert.Equal(workingPaper.Title, dtoResult.Title);
                Assert.Equal(workingPaper.Link, dtoResult.Link);
                Assert.Equal(workingPaper.Abstract, dtoResult.Abstract);
            }
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
        public async Task Create_Returns_WorkingPaperDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            WorkingPaper workingPaper = new WorkingPaper() { Id = 1, Title = "Random Title", Link = "Some link", Abstract = "Some abstract" };
            Moq.Mock<IWorkingPaperDto> workingPaperDto = new Mock<IWorkingPaperDto>();
            SetupWorkingPaperDto(workingPaperDto, workingPaper.Id, workingPaper.Title, workingPaper.Link, workingPaper.Abstract);
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.Add(workingPaper));
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetDto(workingPaper.Id)).Returns(workingPaperDto.Object);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult response = await workingPapersController.CreateWorkingPaper(workingPaper);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                IWorkingPaperDto dtoResult = (IWorkingPaperDto)result.Value;
                Assert.Equal(workingPaper.Id, dtoResult.Id);
                Assert.Equal(workingPaper.Title, dtoResult.Title);
                Assert.Equal(workingPaper.Link, dtoResult.Link);
                Assert.Equal(workingPaper.Abstract, dtoResult.Abstract);
            }
        }

        [Fact]
        public void Update_Returns_EditedWorkingPaperDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            WorkingPaper workingPaper = new WorkingPaper() { Id = 1, Title = "Random Title", Link = "Some link", Abstract = "Some abstract" };
            string titlePatch = "Edited Title";
            JsonPatchDocument<WorkingPaper> workingPaperPatch = new JsonPatchDocument<WorkingPaper>().Replace(wp => wp.Title, titlePatch);
            Moq.Mock<IWorkingPaperDto> workingPaperDto = new Mock<IWorkingPaperDto>();
            SetupWorkingPaperDto(workingPaperDto, workingPaper.Id, titlePatch, workingPaper.Link, workingPaper.Abstract);
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetById(workingPaper.Id)).Returns(workingPaper);
            mockIUnitOfWork.Setup(unit => unit.WorkingPapers.GetDto(workingPaper.Id)).Returns(workingPaperDto.Object);
            WorkingPapersController workingPapersController = new WorkingPapersController(mockIUnitOfWork.Object);

            IActionResult response = workingPapersController.UpdateWorkingPaper(workingPaper.Id, workingPaperPatch);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                IWorkingPaperDto dtoResult = (IWorkingPaperDto)result.Value;
                Assert.Equal(workingPaper.Id, dtoResult.Id);
                Assert.Equal(workingPaper.Title, dtoResult.Title);
                Assert.Equal(workingPaper.Abstract, dtoResult.Abstract);
                Assert.Equal(workingPaper.Link, dtoResult.Link);
            }
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

        private void SetupWorkingPaperDto(Mock<IWorkingPaperDto> workingPaperDto, int Id, string Title, string Link, string Abstract)
        {
            workingPaperDto.SetupAllProperties();
            workingPaperDto.Object.Id = Id;
            workingPaperDto.Object.Title = Title;
            workingPaperDto.Object.Link = Link;
            workingPaperDto.Object.Abstract = Abstract;
        }
    }
}