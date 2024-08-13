using AutoMapper;
using VolleyLeague.Entities.Models;
using VolleyLeague.Shared.Dtos.Discussion;

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
