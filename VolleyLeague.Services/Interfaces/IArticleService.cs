using VolleyLeague.Entities.Dtos.Discussion;

namespace VolleyLeague.Services.Services
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetAllArticles();
        void AddArticle(ArticleDto articleDto);
        Task<ArticleDto> GetArticleById(int id);
        Task<List<ArticleDto>> GetArticlesPerPage(int page);
    }

}

