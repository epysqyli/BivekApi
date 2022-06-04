using Api.Interfaces;

namespace Api.Models
{
    public class TagDto : ITagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}