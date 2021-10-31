using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;

namespace MainApplicationService.Interfaces
{
    public interface IArticlesRepository : IBaseRepository<Article>
    {
        public Task<(IEnumerable<Article> Results, int TotalCount)> GetListAsync(int skip, int take, CancellationToken cancellationToken = default);
    }
}
