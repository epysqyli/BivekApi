using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Tag : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; }
    }
}