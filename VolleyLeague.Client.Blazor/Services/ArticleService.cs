using System.Net.Http.Json;
using System.Text.Json;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.Client.Blazor.Services
{
    public interface IArticleService
    {
        public Task<bool> AddArticle(ArticleDto article);
        public Task<ArticleDto> GetArticleById(int id);

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

        public async Task<ArticleDto> GetArticleById(int id)
        {
            var response = await _httpClient.GetAsync($"api/article/GetArticleById/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ArticleDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }
    }
}
