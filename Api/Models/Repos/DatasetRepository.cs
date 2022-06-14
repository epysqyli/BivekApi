using Microsoft.EntityFrameworkCore;
using Api.Interfaces;
using Api.Data;

namespace Api.Models
{
    public class DatasetRepository : GenericRepository<Dataset>, IDatasetRepository
    {
        public DatasetRepository(ApiDbContext context) : base(context)
        { }

        public IEnumerable<IDatasetDto> GetAllDtos()
        {
            IEnumerable<int> datasetIds = _context.Datasets.Select(d => d.Id).ToList();
            List<IDatasetDto> datasetDtos = new List<IDatasetDto>();
            foreach (int id in datasetIds)
                datasetDtos.Add(GetDto(id));

            return datasetDtos;
        }

        public IDatasetDto GetDto(int id)
        {
            Dataset dataset = _context.Datasets.Where(d => d.Id == id).Include(d => d.DataCategory).FirstOrDefault();
            if (dataset != null)
                return new DatasetDto
                {
                    Id = dataset.Id,
                    Title = dataset.Title,
                    Link = dataset.Link,
                    CategoryId = dataset.DataCategory.Id,
                };

            return null;
        }
    }
}