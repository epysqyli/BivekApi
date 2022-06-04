namespace Api.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        ICommentRepository Comments { get; }
        IArticleTagRepository ArticleTags { get; }
        ITagRepository Tags { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}