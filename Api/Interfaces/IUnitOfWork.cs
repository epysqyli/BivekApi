namespace Api.Models
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        int Complete();
    }
}