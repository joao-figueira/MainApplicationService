using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Api.Dtos;
using MainApplicationService.Api.StaticMappers;
using MainApplicationService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MainApplicationService.Api.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1")]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly IArticlesRepository _articlesRepository;

        public ArticleController(IArticlesRepository articlesRepository)
        {
            _articlesRepository = articlesRepository;
        }

        /// <summary>
        /// Outside the scope. The focus should be on the CommentController.
        /// For tests support only. 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("articles")]
        [ProducesResponseType(typeof(ArticlesListDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, List<string>>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        {
            var (results, totalCount) = await _articlesRepository.GetListAsync(skip, take, cancellationToken);
            return Ok(new ArticlesListDto()
            {
                Results = results.Select(c => c.ToArticleDto()),
                TotalCount = totalCount
            });
        }
    }
}
