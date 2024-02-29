using Microsoft.AspNetCore.Mvc;
using Volleyball.DTO.Discussion;
using VolleyLeague.Entities.Dtos.Discussion;
using VolleyLeague.Services.Services;

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

        [HttpPost("AddArticle")]
        public IActionResult AddArticle([FromBody] ArticleDto article)
        {
            _articleService.AddArticle(article);
            return Ok();
        }
        [HttpGet("GetArticlesPerPage/{page}")]
        public async Task<IActionResult> GetArticlesPerPage(int page)
        {
            var result = await _articleService.GetArticlesPerPage(page);
            return Ok(result);
        }


    }
}