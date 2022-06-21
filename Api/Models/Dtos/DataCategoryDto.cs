using Api.Interfaces;

namespace Api.Models.Dtos
{
    public class DataCategoryDto : IDataCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IDatasetDto> Datasets { get; set; }
    }
}