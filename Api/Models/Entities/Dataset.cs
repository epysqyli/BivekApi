using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Dataset
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        public int DataCategoryId { get; set; }
        public DataCategory DataCategory { get; set; }
    }
}