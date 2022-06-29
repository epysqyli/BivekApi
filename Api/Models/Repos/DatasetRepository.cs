using Microsoft.EntityFrameworkCore;
using Api.Interfaces;
using Api.Data;
using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Models.Repositories
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
            if (dataset == null)
                return null;

            return new DatasetDto
            {
                Id = dataset.Id,
                Title = dataset.Title,
                Link = dataset.Link,
                DataCategoryId = dataset.DataCategory.Id,
            };
        }
    }
}