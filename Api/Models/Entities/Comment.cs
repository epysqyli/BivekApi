using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    public class Comment : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
       
        public string Author {get; set;}

        public Article Article { get; set; }
    }
}
