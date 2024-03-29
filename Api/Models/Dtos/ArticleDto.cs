using Api.Data;
using Api.Models.Entities;
using Api.Interfaces;

namespace Api.Models.Dtos
{
    public class ArticleDto : IArticleDto
    {
        private ApiDbContext _context;
        private Article _article;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public List<TagDto> Tags { get; set; }

        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool Published { get; set; }

        public ArticleDto(int ArticleId, ApiDbContext context)
        {
            _context = context;
            Id = ArticleId;
            _article = getArticle();

            if (_article != null)
            {
                assignTitle();
                assignBody();
                assignTags();
                assignTimestamps();
                assignPublishedStatus();
            }
            else
            {
                Id = 0;
            }
        }

        private Article getArticle()
        {
            return _context.Articles.Find(Id);
        }

        private void assignTitle() => Title = _article.Title;
        private void assignBody() => Body = _article.Body;

        private void assignTags()
        {
            Tags = _context.ArticleTags.Where(at => at.ArticleId == Id)
                                       .Select(at => at.Tag)
                                       .Select(t => new TagDto() { Id = t.Id, Name = t.Name })
                                       .ToList();
        }

        private void assignTimestamps()
        {
            CreatedAt = _article.CreatedAt?.ToString("dd MMMM, yyyy");
            UpdatedAt = _article.UpdatedAt?.ToString("dd MMMM, yyyy");
        }

        private void assignPublishedStatus()
        {
            Published = _article.Published;
        }

        public bool isNull() => Id == 0 ? true : false;
    }
}