using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}
