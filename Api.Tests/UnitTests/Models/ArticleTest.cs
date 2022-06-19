using Xunit;
using Api.Models;
using Api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Api.UnitTests.Models
{
    public class ArticleTest
    {
        [Fact]
        public void TitleIsAssigned()
        {
            Article article = new Article()
            {
                Title = "Test Title",
                Body = "Test body for the article"
            };

            Assert.Same("Test Title", article.Title);
        }

        [Fact(Skip = "needs integration testing, validation is triggered at runtime")]
        public void ArticleWithDuplicateTitleIsNotCreated()
        {
            DbContextOptions<ApiDbContext> options = new DbContextOptionsBuilder<ApiDbContext>()
                                                         .UseInMemoryDatabase(databaseName: "BlogDatabase").Options;

            string title = "New Article Title";
            Article firstArticle = new Article() { Title = title, Body = "some content" };
            Article secondArticle = new Article() { Title = title, Body = "some other content" };

            using (ApiDbContext context = new ApiDbContext(options))
            {
                context.Articles.Add(firstArticle);
                context.SaveChanges();
                context.Articles.Add(secondArticle);
                context.SaveChanges();
                Assert.Equal(1, context.Articles.Count());
            }
        }
    }
}