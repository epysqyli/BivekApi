using Api.Data;
using Api.Interfaces;

namespace Api.Models
{
    public class ArticleDto
    {
        private  IUnitOfWork _unitOfWork;
        private Article _article;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public List<TagDto> Tags { get; set; }
        public List<CommentDto> Comments { get; set; }

        private ArticleDto(int ArticleId, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Id = ArticleId;
        }

        public static async Task<ArticleDto> Create(int ArticleId, IUnitOfWork unitOfWork)
        {
            ArticleDto articleDto = new ArticleDto(ArticleId, unitOfWork);
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
            return _unitOfWork.Articles.GetById(Id);
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
            return await Task.Run(() => _unitOfWork.ArticleTags.Find(at => at.ArticleId == Id)
                                       .Select(at => at.Tag)
                                       .Select(t => new TagDto() { Id = t.Id, Name = t.Name }).ToList());
        }

        private async Task<List<CommentDto>> assignComments()
        {
            return await Task.Run(() => _unitOfWork.Comments.Find(c => c.Article == _article)
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