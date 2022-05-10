using Api.Data;

namespace Api.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;
        public IArticleRepository Articles { get; private set; }
        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            Articles = new ArticleRepository(_context);
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