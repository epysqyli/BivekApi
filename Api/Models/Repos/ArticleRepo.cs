using Api.Data;

namespace Api.Models
{
    public class ArticleRepo : IArticleRepository
    {
        private ApiDbContext _context;

        public ArticleRepo(ApiDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Article> GetArticles()
        {
            return _context.Articles.ToList<Article>();
        }

        public Article GetArticleById(int articleId)
        {
            return _context.Articles.Find(articleId);
        }

        public void InsertArticle(Article article)
        {
            _context.Articles.Add(article);
        }

        public void UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
        }

        public void DeleteArticle(int articleId)
        {
            Article article = _context.Articles.Find(articleId);
            _context.Articles.Remove(article);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}