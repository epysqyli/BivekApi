namespace Api.Interfaces
{
    public interface IDataCategoryDto
    {
        int Id { get; set; }
        string Name { get; set; }
        IEnumerable<IDatasetDto> Datasets { get; set; }
    }
}