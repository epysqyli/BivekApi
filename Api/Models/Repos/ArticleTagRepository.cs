using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class ArticleTagRepository : GenericRepository<ArticleTag>, IArticleTagRepository
    {
        public ArticleTagRepository(ApiDbContext context) : base(context)
        { }
    }
}