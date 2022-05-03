namespace Api.Models
{
    public class CommentDto
    {
        public CommentDto() { }
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}