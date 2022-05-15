using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;
        public IArticleRepository Articles { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public IArticleTagRepository ArticleTags { get; private set; }
        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Articles = new ArticleRepository(_context);
            Comments = new CommentRepository(_context);
            ArticleTags = new ArticleTagRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}