using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Discussion;

namespace VolleyLeague.Services.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Article> _articleRepository;
        private readonly ILogService _logService;

        public ArticleService(IBaseRepository<Article> articleRepository, ILogService logService, IMapper mapper)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
            _logService = logService;
        }

        public async Task<List<ArticleDto>> GetAllArticles()
        {
            var result = await _articleRepository.GetAll().Include(a => a.Author).OrderByDescending(a => a.CreationDate).ToListAsync();

            var allArticles = _mapper.Map<List<ArticleDto>>(result);
            return allArticles;
        }



        public async Task<ArticleDto> GetArticleById(int id)
        {
            var result = await _articleRepository.GetAll().Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == id);
            var articleDto = _mapper.Map<ArticleDto>(result);

            return articleDto;
        }

        public async Task AddArticle(ArticleDto articleDto)
        {
            Article articleNew = new Article();
            articleNew = _mapper.Map<Article>(articleDto);
            articleNew.AuthorId = 133;
            await _articleRepository.InsertAsync(articleNew);
            await _articleRepository.SaveChangesAsync();
            await _logService.AddLog("Nowy artykuł: " + articleNew.Title, "Strony/Artykul.aspx?id=" + articleNew.Id, false, null);
        }
        public async Task<List<ArticleDto>> GetArticlesPerPage(int page)
        {
            int pageSize = 9;
            var result = await _articleRepository.GetAll()
                .Include(a => a.Author)
                .OrderByDescending(a => a.CreationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var articlesPerPage = _mapper.Map<List<ArticleDto>>(result);
            return articlesPerPage;
        }

        public async Task<List<ArticleDto>> SearchArticlesByContentAsync(string searchTerm)
        {
            var articles = await _articleRepository.GetAll()
                                                   .Where(a => EF.Functions.Like(a.Content, $"%{searchTerm}%"))
                                                   .Include(a => a.Author)
                                                   .OrderByDescending(a => a.CreationDate)
                                                   .ToListAsync();

            var articleDtos = _mapper.Map<List<ArticleDto>>(articles);
            return articleDtos;
        }


        public async Task<List<MinimalArticleDto>> GetRecentArticlesAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            var result = await _articleRepository.GetAll()
                .AsNoTracking()
                .OrderByDescending(a => a.CreationDate)
                .Take(3)
                .Select(a => new MinimalArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    CreationDate = a.CreationDate,
                    Image = a.Image
                })
                .ToListAsync();

            stopwatch.Stop();
            Console.WriteLine($"Query Execution Time: {stopwatch.ElapsedMilliseconds} ms");

            return result;
        }
    }
}

