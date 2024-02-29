﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VolleyLeague.Entities.Dtos.Discussion;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;

namespace VolleyLeague.Services.Services
{
    public class ArticleService : IArticleService
    {
            private readonly IMapper _mapper;
            private readonly IBaseRepository<Article> _articleRepository;
            
        public ArticleService(IBaseRepository<Article> articleRepository, IMapper mapper)
            {
                _mapper = mapper;
                _articleRepository = articleRepository;
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
            var articleDto = _mapper.Map<ArticleDto> (result);

            return articleDto;
        }

        public void AddArticle(ArticleDto articleDto)
        {
            Article articleNew = new Article();
            articleNew.AuthorId = 133;

            var articleToDb = _mapper.Map(articleNew, articleDto);
            Console.WriteLine(articleToDb);            
        }
        public async Task<List<ArticleDto>> GetArticlesPerPage(int page)
        {
            int pageSize = 8;
            var result = await _articleRepository.GetAll()
                .Include(a => a.Author)
                .OrderByDescending(a => a.CreationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var articlesPerPage = _mapper.Map<List<ArticleDto>>(result);
            return articlesPerPage;
        }
    }
}

