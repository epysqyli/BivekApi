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

        [Required]
        public bool Published { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }
    }
}