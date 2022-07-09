using Xunit;
using Api.Models.Entities;
using Api.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Api.UnitTests.Models
{
    public class ArticleTest
    {
        [Fact]
        public void ArticleIsCreated()
        {
            DbContextOptions<ApiDbContext> options = new DbContextOptionsBuilder<ApiDbContext>()
                                                         .UseInMemoryDatabase(databaseName: "BlogDatabase").Options;

            Article article = new Article() { Title = "New Article Title", Body = "some content" };

            using (ApiDbContext context = new ApiDbContext(options))
            {
                context.Articles.Add(article);
                context.SaveChanges();
                Assert.Equal(1, context.Articles.Count());
            }
        }
    }
}