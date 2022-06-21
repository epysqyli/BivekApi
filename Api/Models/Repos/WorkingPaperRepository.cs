using Api.Data;
using Api.Interfaces;
using Api.Models.Entities;
using Api.Models.Dtos;

namespace Api.Models.Repositories
{
    public class WorkingPaperRepository : GenericRepository<WorkingPaper>, IWorkingPaperRepository
    {
        public WorkingPaperRepository(ApiDbContext context) : base(context)
        { }

        public IEnumerable<IWorkingPaperDto> GetAllDtos()
        {
            IEnumerable<int> workingPaperIds = _context.WorkingPapers.Select(wp => wp.Id).ToList();
            List<IWorkingPaperDto> workingPaperDtos = new List<IWorkingPaperDto>();
            foreach (int id in workingPaperIds)
                workingPaperDtos.Add(GetDto(id));

            return workingPaperDtos;
        }

        public IWorkingPaperDto GetDto(int id)
        {
            WorkingPaper workingPaper = _context.WorkingPapers.Where(wp => wp.Id == id).FirstOrDefault();
            return new WorkingPaperDto()
            {
                Id = workingPaper.Id,
                Title = workingPaper.Title,
                Abstract = workingPaper.Abstract,
                Link = workingPaper.Link
            };
        }
    }
}