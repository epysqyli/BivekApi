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
        public ITagRepository Tags { get; private set; }
        public IDatasetRepository Datasets { get; private set; }
        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Articles = new ArticleRepository(_context);
            Comments = new CommentRepository(_context);
            ArticleTags = new ArticleTagRepository(_context);
            Tags = new TagRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}