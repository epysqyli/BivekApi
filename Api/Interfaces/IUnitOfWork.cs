namespace Api.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        int Complete();
    }
}