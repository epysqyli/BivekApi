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
    public class ArticlesControllerTest
    {
        [Fact]
        public void GetArticle_Returns_NotFound_GivenNoArticle()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Moq.Mock<IArticleDto> articleDto = new Mock<IArticleDto>();
            articleDto.Setup(a => a.isNull()).Returns(true);
            int someArticleId = 1;
            mockIUnitOfWork.Setup(unit => unit.Articles.GetDto(someArticleId)).Returns(articleDto.Object);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = articlesController.GetArticle(someArticleId);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public void GetArticle_Returns_OkObjectResult()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Id = 1, Title = "Some Title", Body = "Some body" };
            Moq.Mock<IArticleDto> articleDto = new Mock<IArticleDto>();
            articleDto.SetupAllProperties();
            articleDto.Object.Id = article.Id;
            articleDto.Object.Title = article.Title;
            articleDto.Object.Body = article.Body;
            mockIUnitOfWork.Setup(unit => unit.Articles.GetDto(article.Id)).Returns(articleDto.Object);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = articlesController.GetArticle(article.Id);

            Assert.IsType<OkObjectResult>(res);
            OkObjectResult okResult = (OkObjectResult)res;
            if (okResult.Value != null)
            {
                IArticleDto dtoResult = (IArticleDto)okResult.Value;
                Assert.Equal(article.Id, dtoResult?.Id);
                Assert.Equal(article.Title, dtoResult?.Title);
                Assert.Equal(article.Body, dtoResult?.Body);
            }
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            IUnitOfWork mock = mockIUnitOfWork.Object;
            ArticlesController articlesController = new ArticlesController(mock);
            articlesController.ModelState.AddModelError("error", "generic error");

            Article article = new Article() { Title = null };
            IActionResult res = await articlesController.CreateArticle(article);

            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async Task Create_Returns_CreatedAtAction()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Title = "Test Title", Body = "Test body" };
            mockIUnitOfWork.Setup(unit => unit.Articles.Add(article));
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = await articlesController.CreateArticle(article);

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
        public async Task Delete_Returns_NoContent()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Id = 1, Title = "Test Title", Body = "Test body" };
            mockIUnitOfWork.Setup(unit => unit.Articles.GetById(article.Id)).Returns(article);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = await articlesController.DeleteArticle(article.Id);

            Assert.IsType<NoContentResult>(res);
        }
    }
}