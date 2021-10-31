using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;

namespace MainApplicationService.Interfaces
{
    /**
     * Note that this interface implements all the IBaseRepository methods plus the ones that are only specific to this kind of entity 
     */
    public interface ICommentsRepository : IBaseRepository<Comment>
    {
        public Task<IEnumerable<Comment>> GetListByParentAsync(string parentEntityId, int skip, int take, bool onlyNew = false, CancellationToken cancellationToken = default);
    }
}
