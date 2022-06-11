using Api.Interfaces;
using Api.Data;

namespace Api.Models
{
    public class DatasetRepository : GenericRepository<Dataset>, IDatasetRepository
    {
        public DatasetRepository(ApiDbContext context) : base(context)
        { }
    }
}