using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

using Api.Models;
using Api.Interfaces;
using Api.Controllers;

namespace Api.UnitTests.Controllers
{
    public class ArticleTagsControllerTest
    {
        [Fact]
        public void GetArticle_Returns_OkObjectResult()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Id = 1, Title = "Some Title", Body = "Some body" };
            Tag tag = new Tag() { Id = 1, Name = "tag" };
            ArticleTag articleTag = new ArticleTag() { ArticleId = article.Id, TagId = tag.Id };
            List<IArticleDto> articleDtos = new List<IArticleDto>();
            mockIUnitOfWork.Setup(unit => unit.Articles.GetArticlesByTagId(tag.Id)).Returns(articleDtos);
            ArticleTagsController articleTagsController = new ArticleTagsController(mockIUnitOfWork.Object);

            IActionResult res = articleTagsController.GetArticlesByTagId(tag.Id);

            Assert.IsType<OkObjectResult>(res);
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
        public async Task Create_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            ArticleTag articleTag = new ArticleTag() { ArticleId = 1, TagId = 1 };
            mockIUnitOfWork.Setup(unit => unit.Articles.AddTagToArticle(articleTag));
            ArticleTagsController articleTagsController = new ArticleTagsController(mockIUnitOfWork.Object);

            IActionResult res = await articleTagsController.CreateArticleTagRelation(articleTag);

            Assert.IsType<CreatedAtActionResult>(res);
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
    }
}