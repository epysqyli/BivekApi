using Microsoft.EntityFrameworkCore;
using Api.Interfaces;
using Api.Data;
using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Models.Repositories
{
    public class DataCategoryRepository : GenericRepository<DataCategory>, IDataCategoryRepository
    {
        public DataCategoryRepository(ApiDbContext context) : base(context)
        { }

        public IEnumerable<IDataCategoryDto> GetAllDtos()
        {
            List<int> ids = _context.DataCategories.Select(dc => dc.Id).ToList();
            return ids.Select(id => GetDto(id));
        }

        public IDataCategoryDto GetDto(int id)
        {
            DataCategory dataCategory = _context.DataCategories.Find(id);
            if (dataCategory == null)
                return null;
            
            return new DataCategoryDto
            {
                Id = dataCategory.Id,
                Name = dataCategory.Name,
                Datasets = GetDatasetDtos(id),
            };
        }

        private IEnumerable<IDatasetDto> GetDatasetDtos(int dataCategoryId)
        {
            IEnumerable<int> datasetIds = _context.Datasets.Where(d => d.DataCategoryId == dataCategoryId)
                                                           .OrderByDescending(a => a.Title).Select(d => d.Id);
            return _context.Datasets.Where(d => datasetIds.Contains(d.Id)).Select(d => new DatasetDto
            {
                Id = d.Id,
                Title = d.Title,
                Link = d.Link,
                DataCategoryId = d.DataCategoryId,
            });
        }
    }
}