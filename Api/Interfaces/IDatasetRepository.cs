using Api.Models.Entities;

namespace Api.Interfaces
{
    public interface IDatasetRepository : IGenericRepository<Dataset>, IDtoRepository<IDatasetDto>
    { }
}