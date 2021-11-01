using System.Collections.Generic;

namespace MainApplicationService.Api.Dtos
{
    public class ArticleDto
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
    }

    public class ArticlesListDto
    {
        public IEnumerable<ArticleDto>? Results { get; set; }
        public int? TotalCount { get; set; }
    }

    public class ArticlePostModel
    {
        public string? Title { get; set; }
    }
}
