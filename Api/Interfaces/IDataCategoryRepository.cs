using Api.Models;

namespace Api.Interfaces
{
    public interface IDataCategoryRepository : IGenericRepository<DataCategory>, IDtoRepository<IDataCategoryDto>
    { }
}