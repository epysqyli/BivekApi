using Api.Models;

namespace Api.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Article>, IDtoRepository<IArticleDto>
    {
        IEnumerable<IArticleDto> GetAllPublishedDtos();
        IEnumerable<IArticleDto> GetArticlesByTagId(int id);
    }
}