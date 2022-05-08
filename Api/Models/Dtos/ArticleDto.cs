using Api.Data;

namespace Api.Models
{
    public class ArticleDto
    {
        private ApiDbContext _context;
        private Article _article;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public List<TagDto> Tags { get; set; }
        public List<CommentDto> Comments { get; set; }

        private ArticleDto(int ArticleId, ApiDbContext context)
        {
            _context = context;
            Id = ArticleId;
        }

        public static async Task<ArticleDto> Create(int ArticleId, ApiDbContext context)
        {
            ArticleDto articleDto = new ArticleDto(ArticleId, context);
            Article article = await articleDto.getArticle();
            if (article != null)
            {
                articleDto._article = article;
                await articleDto.Initialize();
                return articleDto;
            }
            
            return null;
        }

        private async Task Initialize()
        {
            assignTitle();
            assignBody();
            Tags = await assignTags();
            Comments = await assignComments();
        }

        private async Task<Article> getArticle()
        {
            return await _context.Articles.FindAsync(Id);
        }

        private void assignTitle()
        {
            Title = _article.Title;
        }

        private void assignBody()
        {
            Body = _article.Body;
        }

        private async Task<List<TagDto>> assignTags()
        {
            return await Task.Run(() => _context.ArticleTags.Where(at => at.ArticleId == Id)
                                       .Select(at => at.Tag)
                                       .Select(t => new TagDto() { Id = t.Id, Name = t.Name }).ToList());
        }

        private async Task<List<CommentDto>> assignComments()
        {
            return await Task.Run(() => _context.Comments.Where(c => c.Article == _article)
                                                         .Select(c => new CommentDto()
                                                         {
                                                             Id = c.Id,
                                                             Author = c.Author,
                                                             Content = c.Content,
                                                         })
                                                         .ToList());
        }
    }
}