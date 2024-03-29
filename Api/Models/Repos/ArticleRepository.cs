using Api.Data;
using Api.Interfaces;
using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Models.Repositories
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ApiDbContext context) : base(context)
        { }

        public IEnumerable<IArticleDto> GetAllDtos()
        {
            IEnumerable<int> articleIds = _context.Articles.OrderByDescending(a => a.CreatedAt).Select(a => a.Id).ToList();
            List<IArticleDto> articleDtos = new List<IArticleDto>();
            foreach (int id in articleIds)
                articleDtos.Add(GetDto(id));

            return articleDtos;
        }

        public IEnumerable<IArticleDto> GetAllPublishedDtos()
        {
            IEnumerable<int> articleIds = _context.Articles.Where(a => a.Published == true)
                                                           .OrderByDescending(a => a.CreatedAt)
                                                           .Select(a => a.Id).ToList();
            List<IArticleDto> articleDtos = new List<IArticleDto>();
            foreach (int id in articleIds)
                articleDtos.Add(GetDto(id));

            return articleDtos;
        }

        public IEnumerable<IArticleDto> GetArticlesByTagId(int id)
        {
            return _context.ArticleTags.Where(at => at.TagId == id).ToList()
                                       .Select(t => GetDto(t.ArticleId))
                                       .OrderByDescending(a => a.CreatedAt);
        }

        public IEnumerable<IArticleDto> GetArticlesByTagIds(int[] ids)
        {
            return _context.ArticleTags.Where(at => ids.Contains(at.TagId)).ToList()
                                       .Select(t => GetDto(t.ArticleId))
                                       .DistinctBy(a => a.Id).Where(a => a.Published)
                                       .OrderByDescending(a => a.CreatedAt).Take(4);
        }

        public IArticleDto GetDto(int id)
        {
            return new ArticleDto(id, _context);
        }

        public async Task AddTagToArticle(ArticleTag articleTag)
        {
            await _context.ArticleTags.AddAsync(articleTag);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTagFromArticle(int articleId, int tagId)
        {
            ArticleTag articleTag = _context.ArticleTags.Where(at => at.TagId == tagId && at.ArticleId == articleId).FirstOrDefault();
            _context.ArticleTags.Remove(articleTag);
            if (articleTag != null)
                await _context.SaveChangesAsync();
        }
    }
}