using AutoMapper;
using VolleyLeague.Entities.Dtos.Discussion;
using VolleyLeague.Entities.Models;

namespace VolleyLeague.Services.Mapping
{
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile() 
            : base() 
        {
            CreateMap<Article, ArticleDto>()
                .ReverseMap();
            CreateMap<User, AuthorInfoDto>();
        }

    }
}
