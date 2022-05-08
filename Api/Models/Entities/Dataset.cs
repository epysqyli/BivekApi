using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Dataset
    {
        public int Id { get; set; }

        [Required]
        public string Link { get; set; }

        public DataCategory Category { get; set; }
    }
}