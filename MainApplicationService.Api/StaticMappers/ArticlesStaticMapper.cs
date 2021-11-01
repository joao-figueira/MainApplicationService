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
                Title = article.Title
            };
        }
    }
}
