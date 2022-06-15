using Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Tag : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [UniqueTagName]
        public string Name { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; }
    }

    public class UniqueTagNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApiDbContext context = (ApiDbContext)validationContext.GetService(typeof(ApiDbContext));
            if (context.Tags.Any(t => t.Name == value.ToString()))
                return new ValidationResult("Name is not unique");

            return ValidationResult.Success;
        }
    }
}