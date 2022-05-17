using Api.Models;

namespace Api.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        IArticleDto GetDto(int id);
    }
}