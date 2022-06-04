using Api.Models;

namespace Api.Interfaces
{
    public interface IArticleDto
    {
        int Id { get; set; }
        string Title { get; set; }
        string Body { get; set; }

        List<TagDto> Tags { get; set; }
        List<CommentDto> Comments { get; set; }

        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        bool Published { get; set; }
        bool isNull();
    }
}