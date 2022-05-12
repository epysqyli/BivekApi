using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

using Api.Models;
using Api.Interfaces;
using Api.Controllers;

namespace Api.UnitTests.Controllers
{
    public class ArticlesControllerTest
    {
        [Fact]
        public void GetArticle_Returns_HttpNotFound_GivenNotexistentArticle()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            int fakeArticleId = 123;
            mockIUnitOfWork.Setup(unit => unit.Articles.GetById(fakeArticleId)).Returns<Article>(null);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = articlesController.GetArticle(fakeArticleId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void GetArticle_Returns_OkObjectResult()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Id = 1, Title = "Some Title", Body = "Some body" };
            mockIUnitOfWork.Setup(unit => unit.Articles.GetById(article.Id)).Returns(article);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = articlesController.GetArticle(article.Id);

            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public void Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            ArticlesController articlesController = new ArticlesController(mock);
            articlesController.ModelState.AddModelError("error", "generic error");

            Article article = new Article() { Title = null };
            IActionResult res = articlesController.CreateArticle(article);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public void Create_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Title = "Test Title", Body = "Test body" };
            mockIUnitOfWork.Setup(unit => unit.Articles.Add(article));
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = articlesController.CreateArticle(article);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void Update_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Id = 1, Title = "Test Title", Body = "Test body" };
            mockIUnitOfWork.Setup(unit => unit.Articles.GetById(article.Id)).Returns(article);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);
            JsonPatchDocument<Article> articlePatch = new JsonPatchDocument<Article>();
            articlePatch.Replace(a => a.Title, "Edited Title");

            IActionResult res = articlesController.UpdateArticle(1, articlePatch);

            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public void Delete_Returns_NoContent()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Id = 1, Title = "Test Title", Body = "Test body" };
            mockIUnitOfWork.Setup(unit => unit.Articles.GetById(article.Id)).Returns(article);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = articlesController.DeleteArticle(article.Id);

            Assert.IsType<NoContentResult>(res);
        }
    }
}