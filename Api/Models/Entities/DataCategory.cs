using Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class DataCategory
    {
        public int Id { get; set; }

        [Required]
        [UniqueDataCategoryName]
        public string Name { get; set; }

        public ICollection<Dataset> Datasets { get; set; }
    }

    public class UniqueDataCategoryNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApiDbContext context = validationContext.GetService<ApiDbContext>();
            if (context.DataCategories.Any(dc => dc.Name == value.ToString()))
                return new ValidationResult("Name is not unique");

            return ValidationResult.Success;
        }
    }
}