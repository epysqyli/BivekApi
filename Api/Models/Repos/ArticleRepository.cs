using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ApiDbContext context) : base(context)
        { }

        public IEnumerable<IArticleDto> GetAllDtos()
        {
            IEnumerable<int> articleIds = _context.Articles.OrderByDescending(a => a.UpdatedAt).Select(a => a.Id).ToList();
            List<IArticleDto> articleDtos = new List<IArticleDto>();
            foreach (int id in articleIds)
                articleDtos.Add(GetDto(id));

            return articleDtos;
        }

        public IEnumerable<IArticleDto> GetAllPublishedDtos()
        {
            IEnumerable<int> articleIds = _context.Articles.Where(a => a.Published == true)
                                                           .OrderByDescending(a => a.UpdatedAt)
                                                           .Select(a => a.Id).ToList();
            List<IArticleDto> articleDtos = new List<IArticleDto>();
            foreach (int id in articleIds)
                articleDtos.Add(GetDto(id));

            return articleDtos;
        }

        public IArticleDto GetDto(int id)
        {
            return new ArticleDto(id, _context);
        }
    }
}