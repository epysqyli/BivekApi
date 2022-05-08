namespace Api.Models
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetArticles();
        Article GetArticleById(int articleId);
        void InsertArticle(Article article);
        void UpdateArticle(Article article);
        void DeleteArticle(int articleId);
        void Save();
    }
}