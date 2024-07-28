using Microsoft.AspNetCore.Mvc;
using VolleyLeague.Services.Services;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleService _articleService;

        public ArticleController(ILogger<ArticleController> logger, IArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }

        [HttpGet("GetAllArticles")]
        public async Task<IActionResult> GetAllArticles()
        {
            var result = await _articleService.GetAllArticles();
            return Ok(result);
        }

        [HttpGet("GetArticleById/{Id}")]
        public async Task<IActionResult> GetArticleById(int Id)
        {
            var result = await _articleService.GetArticleById(Id);
            return Ok(result);
        }

        [HttpPost("addArticle")]
        public async Task<IActionResult> AddArticle([FromBody] ArticleDto article)
        {
            article.CreationDate = DateTime.Now;
            await _articleService.AddArticle(article);
            return Ok();
        }
        [HttpGet("GetArticlesPerPage/{page}")]
        public async Task<IActionResult> GetArticlesPerPage(int page)
        {
            var result = await _articleService.GetArticlesPerPage(page);
            return Ok(result);
        }

        [HttpGet("GetRecentArticlesAsync")]
        public async Task<IActionResult> GetRecentArticlesAsync()
        {
            var result = await _articleService.GetRecentArticlesAsync();
            return Ok(result);
        }

        [HttpGet("SearchArticlesByContent/{searchTerm}")]
        public async Task<IActionResult> SearchArticlesByContent(string searchTerm)
        {
            var result = await _articleService.SearchArticlesByContentAsync(searchTerm);
            return Ok(result);
        }
    }
}