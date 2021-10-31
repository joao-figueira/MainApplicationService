using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;

namespace MainApplicationService.Interfaces
{
    public interface IArticlesRepository : IBaseRepository<Article>
    {
        public Task<IEnumerable<Article>> GetListAsync(int skip, int take, CancellationToken cancellationToken = default);
    }
}
