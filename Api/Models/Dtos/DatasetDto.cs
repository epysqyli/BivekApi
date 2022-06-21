using Api.Interfaces;

namespace Api.Models.Dtos
{
    public class DatasetDto : IDatasetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int dataCategoryId { get; set; }
    }
}