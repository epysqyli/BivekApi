using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class ArticleDto : IArticleDto
    {
        private ApiDbContext _context;
        private Article _article;
        public int Id { get; set; }
        public string Title { get => _article.Title; set => Title = _article.Title; }
        public string Body { get => _article.Body; set => Body = _article.Body; }

        public List<TagDto> Tags { get; set; }
        public List<CommentDto> Comments { get; set; }

        public ArticleDto(int ArticleId, ApiDbContext context)
        {
            _context = context;
            Id = ArticleId;
            _article = getArticle();
            assignTags();
            assignComments();
        }

        private Article getArticle()
        {
            return _context.Articles.Find(Id);
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