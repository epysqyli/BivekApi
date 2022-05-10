using System;
using Moq;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Api.Models;
using Api.Interfaces;
using Api.Data;
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
    }
}