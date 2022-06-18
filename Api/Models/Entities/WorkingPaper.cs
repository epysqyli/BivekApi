using Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class WorkingPaper : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [UniqueWorkingPaperTitle]
        public string Title { get; set; }

        public string Abstract { get; set; }

        [Required]
        public string Link { get; set; }
    }

    public class UniqueWorkingPaperTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApiDbContext context = validationContext.GetService<ApiDbContext>();
            if (context.WorkingPapers.Any(wk => wk.Title == value.ToString()))
                return new ValidationResult("Title is not unique");

            return ValidationResult.Success;
        }
    }
}