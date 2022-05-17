using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ApiDbContext context) : base(context)
        { }

        public IArticleDto GetDto(int id)
        {
            return new ArticleDto(id, _context);
        }
    }
}