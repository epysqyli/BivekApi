using Api.Data;
using Microsoft.AspNetCore.JsonPatch;

namespace Api.Models
{
    public class ArticleRepo : IArticleRepository
    {
        private ApiDbContext _context;

        public ArticleRepo(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleDto>> GetArticleDtos()
        {
            IEnumerable<int> ids = _context.Articles.Select(a => a.Id).ToList();
            List<ArticleDto> articles = new List<ArticleDto>();
            foreach (int id in ids)
                articles.Add(await ArticleDto.Create(id, _context));

            return articles;
        }

        public async Task<Article> GetArticleById(int articleId)
        {
            return await _context.Articles.FindAsync(articleId);
        }

        public async Task<ArticleDto> GetArticleDtoById(int articleId)
        {
            return await ArticleDto.Create(articleId, _context);
        }

        public async Task InsertArticle(Article article)
        {
            await _context.Articles.AddAsync(article);
        }

        public void UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
        }

        public void PartialUpdateArticle(Article article, JsonPatchDocument articlePatch)
        {
            articlePatch.ApplyTo(article);
        }

        public async Task DeleteArticle(int articleId)
        {
            Article article = await _context.Articles.FindAsync(articleId);
            _context.Articles.Remove(article);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}