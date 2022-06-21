using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    public class ArticleTag : BaseEntity
    {
        [Required]
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        
        [Required]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}