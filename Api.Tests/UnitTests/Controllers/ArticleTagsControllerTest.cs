using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Api.Models.Entities;
using Api.Models.Dtos;
using Api.Interfaces;
using Api.Controllers;

namespace Api.UnitTests.Controllers
{
    public class ArticleTagsControllerTest
    {
        [Fact]
        public void GetArticlesByTagId_Returns_ArticleDtos()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article firstArticle = new Article() { Id = 1, Title = "First Title", Body = "First Body", ArticleTags = GetArticleTags(1) };
            Article secondArticle = new Article() { Id = 2, Title = "Second Title", Body = "Second Body", ArticleTags = GetArticleTags(1) };
            Tag tag = new Tag() { Id = 1, Name = "First Tag" };
            Moq.Mock<IArticleDto> firstArticleDto = new Mock<IArticleDto>();
            Moq.Mock<IArticleDto> secondArticleDto = new Mock<IArticleDto>();
            List<Mock<IArticleDto>> articleDtos = new List<Mock<IArticleDto>>() { firstArticleDto, secondArticleDto };
            SetupArticleDtos(articleDtos, firstArticle, secondArticle, GetArticleTagDtos());
            mockIUnitOfWork.Setup(unit => unit.Articles.GetArticlesByTagId(tag.Id)).Returns(articleDtos.Select(a => a.Object));
            ArticleTagsController articleTagsController = new ArticleTagsController(mockIUnitOfWork.Object);

            IActionResult response = articleTagsController.GetArticlesByTagId(tag.Id);
            OkObjectResult result = (OkObjectResult)response;

            Assert.IsType<OkObjectResult>(response);
            if (result.Value != null)
            {
                IEnumerable<IArticleDto> dtoResults = (IEnumerable<IArticleDto>)result.Value;
                Assert.Equal(firstArticle.Id, dtoResults.ElementAt(0).Id);
                Assert.Equal(firstArticle.Title, dtoResults.ElementAt(0).Title);
                Assert.Equal(firstArticle.Body, dtoResults.ElementAt(0).Body);
                Assert.Equal(secondArticle.Id, dtoResults.ElementAt(1).Id);
                Assert.Equal(secondArticle.Title, dtoResults.ElementAt(1).Title);
                Assert.Equal(secondArticle.Body, dtoResults.ElementAt(1).Body);
            }
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            ArticleTagsController articleTagsController = new ArticleTagsController(mockIUnitOfWork.Object);
            articleTagsController.ModelState.AddModelError("error", "generic error");

            ArticleTag articleTag = new ArticleTag() { ArticleId = 1 };
            IActionResult res = await articleTagsController.CreateArticleTagRelation(articleTag);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async Task Create_Returns_ArticleTag()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            ArticleTag articleTag = new ArticleTag() { ArticleId = 1, TagId = 1 };
            mockIUnitOfWork.Setup(unit => unit.Articles.AddTagToArticle(articleTag));
            ArticleTagsController articleTagsController = new ArticleTagsController(mockIUnitOfWork.Object);

            IActionResult response = await articleTagsController.CreateArticleTagRelation(articleTag);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            Assert.IsType<CreatedAtActionResult>(response);
            if (result.Value != null)
            {
                ArticleTag atResult = (ArticleTag)result.Value;
                Assert.Equal(articleTag.ArticleId, atResult.ArticleId);
                Assert.Equal(articleTag.TagId, atResult.TagId);
            }
        }

        [Fact]
        public async Task Delete_Returns_NoContent()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            ArticleTag articleTag = new ArticleTag() { ArticleId = 1, TagId = 1 };
            mockIUnitOfWork.Setup(unit => unit.Articles.RemoveTagFromArticle(articleTag.ArticleId, articleTag.TagId));
            ArticleTagsController articleTagsController = new ArticleTagsController(mockIUnitOfWork.Object);

            IActionResult res = await articleTagsController.DeleteArticleTagRelation(articleTag.ArticleId, articleTag.TagId);

            Assert.IsType<NoContentResult>(res);
        }

        private ICollection<ArticleTag> GetArticleTags(int Id)
        {
            return new List<ArticleTag> { new ArticleTag() { ArticleId = Id, TagId = 1 } };
        }

        private List<TagDto> GetArticleTagDtos()
        {
            return new List<TagDto> { new TagDto() { Id = 1, Name = "First Tag" } };
        }

        private void SetupArticleDtos(List<Mock<IArticleDto>> articleDtos, Article firstArticle, Article secondArticle, List<TagDto> tagDtos)
        {
            articleDtos[0].SetupAllProperties();
            articleDtos[0].Object.Id = firstArticle.Id;
            articleDtos[0].Object.Title = firstArticle.Title;
            articleDtos[0].Object.Body = firstArticle.Body;
            articleDtos[0].Object.Tags = tagDtos;

            articleDtos[1].SetupAllProperties();
            articleDtos[1].Object.Id = secondArticle.Id;
            articleDtos[1].Object.Title = secondArticle.Title;
            articleDtos[1].Object.Body = secondArticle.Body;
            articleDtos[1].Object.Tags = tagDtos;
        }
    }
}