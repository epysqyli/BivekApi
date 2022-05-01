using Xunit;
using Api.Models;

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
    }
}