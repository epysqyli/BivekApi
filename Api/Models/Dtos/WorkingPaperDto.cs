using Api.Interfaces;

namespace Api.Models.Dtos
{
    public class WorkingPaperDto : IWorkingPaperDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Link { get; set; }
        public string DatasetLink { get; set; }
        public string CreatedAt { get; set; }
    }
}