using Api.Models;

namespace Api.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        IEnumerable<IArticleDto> GetAllDtos();
        IEnumerable<IArticleDto> GetAllPublishedDtos();
        IArticleDto GetDto(int id);
    }
}