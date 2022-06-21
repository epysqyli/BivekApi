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
    public class TagsControllerTest
    {
        [Fact]
        public void GetTag_Returns_NotFound_GivenNoTag()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            int tagId = 1;
            mockIUnitOfWork.Setup(unit => unit.Tags.GetDto(tagId)).Returns<ITagDto>(null);
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);

            IActionResult res = tagsController.GetTag(tagId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void GetTag_Returns_OkObjectResult()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<ITagDto> tagDto = new Mock<ITagDto>();
            Tag tag = new Tag() { Id = 1, Name = "Random Tag" };
            mockIUnitOfWork.Setup(unit => unit.Tags.GetDto(tag.Id)).Returns(tagDto.Object);
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);

            IActionResult res = tagsController.GetTag(tag.Id);

            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);
            tagsController.ModelState.AddModelError("error", "generic error");

            Tag tag = new Tag() { Name = null };
            IActionResult res = await tagsController.CreateTag(tag);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async Task Create_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Tag tag = new Tag() { Id = 1, Name = "Random Tag" };
            mockIUnitOfWork.Setup(unit => unit.Tags.Add(tag));
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);

            IActionResult res = await tagsController.CreateTag(tag);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void Update_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Tag tag = new Tag() { Id = 1, Name = "Random Tag" };
            mockIUnitOfWork.Setup(unit => unit.Tags.GetById(tag.Id)).Returns(tag);
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);
            JsonPatchDocument<Tag> tagPatch = new JsonPatchDocument<Tag>();
            tagPatch.Replace(t => t.Name, "New Title");

            IActionResult res = tagsController.UpdateTag(1, tagPatch);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public async Task Delete_Returns_NoContent()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Tag tag = new Tag() { Id = 1, Name = "Random Tag" };
            mockIUnitOfWork.Setup(unit => unit.Tags.GetById(tag.Id)).Returns(tag);
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);

            IActionResult res = await tagsController.DeleteTag(tag.Id);

            Assert.IsType<NoContentResult>(res);
        }
    }
}