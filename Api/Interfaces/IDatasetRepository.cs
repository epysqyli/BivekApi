using Api.Models;

namespace Api.Interfaces
{
    public interface IDatasetRepository : IGenericRepository<Dataset>
    {
        IEnumerable<IDatasetDto> GetAllDtos();
        IDatasetDto GetDto(int id);
    }
}