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
            IEnumerable<int> workingPaperIds = _context.WorkingPapers.OrderByDescending(wp => wp.CreatedAt)
                                                                     .Select(wp => wp.Id).ToList();
            return workingPaperIds.Select((id) => GetDto(id));
        }

        public IWorkingPaperDto GetDto(int id)
        {
            WorkingPaper workingPaper = _context.WorkingPapers.Where(wp => wp.Id == id).FirstOrDefault();
            if (workingPaper == null)
                return null;

            return new WorkingPaperDto()
            {
                Id = workingPaper.Id,
                Title = workingPaper.Title,
                Abstract = workingPaper.Abstract,
                Link = workingPaper.Link,
                DatasetLink = workingPaper.DatasetLink,
                CreatedAt = workingPaper.CreatedAt?.ToString("dd MMMM, yyyy")
            };
        }
    }
}