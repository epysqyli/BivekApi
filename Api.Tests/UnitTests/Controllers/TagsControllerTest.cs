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
        public void GetTag_Returns_TagDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<ITagDto> tagDto = new Mock<ITagDto>();
            Tag tag = new Tag() { Id = 1, Name = "Random Tag" };
            SetupTagDto(tagDto, tag.Id, tag.Name);
            mockIUnitOfWork.Setup(unit => unit.Tags.GetDto(tag.Id)).Returns(tagDto.Object);
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);

            IActionResult response = tagsController.GetTag(tag.Id);

            Assert.IsType<OkObjectResult>(response);
            OkObjectResult okResult = (OkObjectResult)response;

            if (okResult.Value != null)
            {
                ITagDto dtoResult = (ITagDto)okResult.Value;
                Assert.Equal(tag.Id, dtoResult.Id);
                Assert.Equal(tag.Name, dtoResult.Name);
            }
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
        public async Task Create_Returns_TagDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Tag tag = new Tag() { Id = 1, Name = "Random Tag" };
            Moq.Mock<ITagDto> tagDto = new Mock<ITagDto>();
            SetupTagDto(tagDto, tag.Id, tag.Name);
            mockIUnitOfWork.Setup(unit => unit.Tags.Add(tag));
            mockIUnitOfWork.Setup(unit => unit.Tags.GetDto(tag.Id)).Returns(tagDto.Object);
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);

            IActionResult response = await tagsController.CreateTag(tag);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                ITagDto dtoResult = (ITagDto)result.Value;
                Assert.Equal(tag.Id, dtoResult.Id);
                Assert.Equal(tag.Name, dtoResult.Name);
            }
        }

        [Fact]
        public void Update_Returns_EditedTagDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Tag tag = new Tag() { Id = 1, Name = "Random Tag" };
            string titlePatch = "Edited Title";
            JsonPatchDocument<Tag> tagPatch = new JsonPatchDocument<Tag>().Replace(t => t.Name, titlePatch);
            Moq.Mock<ITagDto> tagDto = new Mock<ITagDto>();
            SetupTagDto(tagDto, tag.Id, titlePatch);
            mockIUnitOfWork.Setup(unit => unit.Tags.GetById(tag.Id)).Returns(tag);
            mockIUnitOfWork.Setup(unit => unit.Tags.GetDto(tag.Id)).Returns(tagDto.Object);
            TagsController tagsController = new TagsController(mockIUnitOfWork.Object);

            IActionResult response = tagsController.UpdateTag(tag.Id, tagPatch);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                ITagDto dtoResult = (ITagDto)result.Value;
                Assert.Equal(tag.Id, dtoResult.Id);
                Assert.Equal(titlePatch, dtoResult.Name);
            }
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

        private void SetupTagDto(Mock<ITagDto> tagDto, int Id, string Name)
        {
            tagDto.SetupAllProperties();
            tagDto.Object.Id = Id;
            tagDto.Object.Name = Name;
        }
    }
}