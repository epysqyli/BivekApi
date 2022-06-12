using Api.Models;

namespace Api.Interfaces
{
    public interface IDataCategoryRepository : IGenericRepository<DataCategory>
    {
        IEnumerable<IDataCategoryDto> GetAllDtos();
        IDataCategoryDto GetDto(int id);
    }
}