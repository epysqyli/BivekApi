using Api.Interfaces;

namespace Api.Models.Dtos
{
    public class TagDto : ITagDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}