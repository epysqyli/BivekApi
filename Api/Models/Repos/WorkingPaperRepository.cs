using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class WorkingPaperRepository : GenericRepository<WorkingPaper>, IWorkingPaperRepository
    {
        public WorkingPaperRepository(ApiDbContext context) : base(context)
        { }
    }
}