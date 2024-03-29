using Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Entities
{
    public class Dataset : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [UniqueDatasetTitle]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        public int DataCategoryId { get; set; }
        public DataCategory DataCategory { get; set; }
    }

    public class UniqueDatasetTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApiDbContext context = validationContext.GetService<ApiDbContext>();
            if (context.Datasets.Any(d => d.Title == value.ToString()))
                return new ValidationResult("Title is not unique");

            return ValidationResult.Success;
        }
    }
}