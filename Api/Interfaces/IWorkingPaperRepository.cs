using Api.Models.Entities;

namespace Api.Interfaces
{
    public interface IWorkingPaperRepository : IGenericRepository<WorkingPaper>, IDtoRepository<IWorkingPaperDto>
    { }
}