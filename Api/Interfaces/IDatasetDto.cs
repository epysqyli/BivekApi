namespace Api.Interfaces
{
    public interface IDatasetDto
    {
        int Id { get; set; }
        string Title { get; set; }
        string Link { get; set; }
        int dataCategoryId { get; set; }
    }
}