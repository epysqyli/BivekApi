
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class WorkingPaper : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Abstract { get; set; }

        [Required]
        public string Link { get; set; }
    }
}