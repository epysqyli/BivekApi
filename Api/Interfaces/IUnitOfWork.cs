namespace Api.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        ITagRepository Tags { get; }
        IDatasetRepository Datasets { get; }
        IDataCategoryRepository DataCategories { get; }
        IWorkingPaperRepository WorkingPapers { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}