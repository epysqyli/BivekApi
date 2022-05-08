using Api.Data;
using System.ComponentModel.DataAnnotations;
namespace Api.Models
{
    public class Article : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }

        public void AddComment(Comment comment, ApiDbContext context)
        {
            comment.Article = this;
            context.Comments.Add(comment);
            context.SaveChanges();
        }

        public override string ToString()
        {
            return $"Title: {this.Title}\nBody: {this.Body}\n";
        }
    }
}