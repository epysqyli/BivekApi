using Api.Models;

namespace Api.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Article>, IDtoRepository<IArticleDto>
    {
        IEnumerable<IArticleDto> GetAllPublishedDtos();
        IEnumerable<IArticleDto> GetArticlesByTagId(int id);
        Task AddTagToArticle(ArticleTag articleTag);
        Task RemoveTagFromArticle(int articleId, int tagId);
    }
}