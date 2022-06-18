using Api.Models;

namespace Api.Interfaces
{
    public interface IWorkingPaperRepository : IGenericRepository<WorkingPaper>, IDtoRepository<IWorkingPaperDto>
    { }
}