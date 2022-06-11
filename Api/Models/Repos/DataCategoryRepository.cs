using Api.Interfaces;
using Api.Data;

namespace Api.Models
{
    public class DataCategoryRepository : GenericRepository<DataCategory>, IDataCategoryRepository
    {
        public DataCategoryRepository(ApiDbContext context) : base(context)
        { }
    }
}