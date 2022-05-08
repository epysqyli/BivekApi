using Microsoft.AspNetCore.JsonPatch;

namespace Api.Models
{
    public interface IArticleRepository
    {
        Task<IEnumerable<ArticleDto>> GetArticleDtos();
        Task<Article> GetArticleById(int articleId);
        Task<ArticleDto> GetArticleDtoById(int articleId);
        Task InsertArticle(Article article);
        void UpdateArticle(Article article);
        void PartialUpdateArticle(Article article, JsonPatchDocument articlePatch);
        Task DeleteArticle(int articleId);
        Task Save();
    }
}