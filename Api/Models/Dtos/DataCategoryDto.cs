using Api.Interfaces;

namespace Api.Models
{
    public class DataCategoryDto : IDataCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IDatasetDto> Datasets { get; set; }
    }
}