using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class DataCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Dataset> Datasets { get; set; }
    }
}