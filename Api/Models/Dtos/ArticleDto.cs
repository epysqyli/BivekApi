using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class ArticleDto : IArticleDto
    {
        private ApiDbContext _context;
        private Article _article;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public List<TagDto> Tags { get; set; }
        public List<CommentDto> Comments { get; set; }

        public ArticleDto(int ArticleId, ApiDbContext context)
        {
            _context = context;
            Id = ArticleId;
            _article = getArticle();
            assignTitle();
            assignBody();
            assignTags();
            assignComments();
        }

        private Article getArticle()
        {
            return _context.Articles.Find(Id);
        }

        private void assignTitle()
        {
            Title = _article.Title;
        }

        private void assignBody()
        {
            Body = _article.Body;
        }

        private void assignTags()
        {
            Tags = _context.ArticleTags.Where(at => at.ArticleId == Id)
                                       .Select(at => at.Tag)
                                       .Select(t => new TagDto() { Id = t.Id, Name = t.Name })
                                       .ToList();
        }

        private void assignComments()
        {
            Comments = _context.Comments.Where(c => c.Article == _article)
                                                         .Select(c => new CommentDto()
                                                         {
                                                             Id = c.Id,
                                                             Author = c.Author,
                                                             Content = c.Content,
                                                         })
                                                         .ToList();
        }
    }
}