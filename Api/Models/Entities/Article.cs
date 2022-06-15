using Api.Data;
using System.ComponentModel.DataAnnotations;
namespace Api.Models
{
    public class Article : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        [UniqueArticleTitle]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public bool Published { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }
    }

    public class UniqueArticleTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApiDbContext context = (ApiDbContext)validationContext.GetService(typeof(ApiDbContext));
            if (context.Articles.Any(a => a.Title == value.ToString()))
                return new ValidationResult("Title is not unique");

            return ValidationResult.Success;
        }
    }
}