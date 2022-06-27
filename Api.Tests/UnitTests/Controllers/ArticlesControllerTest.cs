using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            SetupArticleDto(articleDto, article.Id, article.Title, article.Body);
            mockIUnitOfWork.Setup(unit => unit.Articles.GetDto(article.Id)).Returns(articleDto.Object);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult res = articlesController.GetArticle(article.Id);
            OkObjectResult okResult = (OkObjectResult)res;

            Assert.IsType<OkObjectResult>(res);
            if (okResult.Value != null)
            {
                IArticleDto dtoResult = (IArticleDto)okResult.Value;
                Assert.Equal(article.Id, dtoResult?.Id);
                Assert.Equal(article.Title, dtoResult?.Title);
                Assert.Equal(article.Body, dtoResult?.Body);
            }
        }

        [Fact]
        public void GetArticles_Returns_ListOfArticleDtos()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork.Setup(unitOfWork => unitOfWork.Articles.GetAllDtos()).Returns(GetArticleDtos());
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult response = articlesController.GetArticles();
            OkObjectResult okResult = (OkObjectResult)response;

            if (okResult.Value != null)
            {
                List<IArticleDto> articleDtos = (List<IArticleDto>)okResult.Value;
                Assert.Equal(2, articleDtos.Count);
                Assert.Equal("First article title", articleDtos[0].Title);
                Assert.Equal("First article body", articleDtos[0].Body);
                Assert.Equal("Second article title", articleDtos[1].Title);
                Assert.Equal("Second article body", articleDtos[1].Body);
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
        public async Task Create_Returns_ArticleDto()
        {
            Moq.Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Article article = new Article() { Title = "Test Title", Body = "Test body" };
            Moq.Mock<IArticleDto> articleDto = new Mock<IArticleDto>();
            SetupArticleDto(articleDto, article.Id, article.Title, article.Body);
            mockIUnitOfWork.Setup(unit => unit.Articles.GetDto(article.Id)).Returns(articleDto.Object);
            ArticlesController articlesController = new ArticlesController(mockIUnitOfWork.Object);

            IActionResult response = await articlesController.CreateArticle(article);
            CreatedAtActionResult result = (CreatedAtActionResult)response;

            if (result.Value != null)
            {
                IArticleDto dtoResult = (IArticleDto)result.Value;
                Assert.Equal(article.Id, dtoResult.Id);
                Assert.Equal(article.Title, dtoResult.Title);
                Assert.Equal(article.Body, dtoResult.Body);
            }
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

        private void SetupArticleDto(Mock<IArticleDto> articleDto, int Id, string Title, string Body)
        {
            articleDto.SetupAllProperties();
            articleDto.Object.Id = Id;
            articleDto.Object.Title = Title;
            articleDto.Object.Body = Body;
        }

        private List<IArticleDto> GetArticleDtos()
        {
            Moq.Mock<IArticleDto> firstArticleDto = new Mock<IArticleDto>();
            SetupArticleDto(firstArticleDto, 1, "First article title", "First article body");
            Moq.Mock<IArticleDto> secondArticleDto = new Mock<IArticleDto>();
            SetupArticleDto(secondArticleDto, 2, "Second article title", "Second article body");

            List<IArticleDto> articleDtos = new List<IArticleDto>();
            articleDtos.Add(firstArticleDto.Object);
            articleDtos.Add(secondArticleDto.Object);

            return articleDtos;
        }
    }
}