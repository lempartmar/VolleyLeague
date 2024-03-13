using System.Net.Http.Json;
using VolleyLeague.Entities.Dtos.Discussion;

namespace VolleyLeague.Client.Blazor2.Services
{
    public interface IArticleService
    {
        public Task<bool> AddArticle(ArticleDto article);

    }
    public class ArticleService : IArticleService
    {
        private readonly HttpClient _httpClient;

        public ArticleService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<bool> AddArticle(ArticleDto article)
        {
            article.CreationDate = DateTime.Now;    
            var response = await _httpClient.PostAsJsonAsync("api/article/addArticle", article);
            return response.IsSuccessStatusCode;
        }
    }
}
