using Api.Models.Entities;

namespace Api.Interfaces
{
    public interface IDataCategoryRepository : IGenericRepository<DataCategory>, IDtoRepository<IDataCategoryDto>
    {
        IEnumerable<IDataCategoryDto> GetNonEmptyDtos();
    }
}