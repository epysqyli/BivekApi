using Api.Models;

namespace Api.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        IEnumerable<IArticleDto> GetAllDtos();

        IArticleDto GetDto(int id);
    }
}