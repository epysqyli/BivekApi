using Api.Models.Dtos;

namespace Api.Interfaces
{
    public interface IArticleDto
    {
        int Id { get; set; }
        string Title { get; set; }
        string Body { get; set; }
        List<TagDto> Tags { get; set; }
        string CreatedAt { get; set; }
        string UpdatedAt { get; set; }
        bool Published { get; set; }
        bool isNull();
    }
}