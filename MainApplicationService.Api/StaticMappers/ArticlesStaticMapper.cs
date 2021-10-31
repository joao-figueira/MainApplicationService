using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApplicationService.Api.Dtos;
using MainApplicationService.Entities;

namespace MainApplicationService.Api.StaticMappers
{
    public static class ArticlesStaticMapper
    {
        public static ArticleDto ToArticleDto(this Article article)
        {
            return new()
            {
                Id = article.Id,
                Title = article.Title,
                CreatedOnUtc = article.CreatedOnUtc
            };
        }
    }
}
