using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;

namespace MainApplicationService.Interfaces
{
    public interface IEntityBaseRepository
    {
        Task<EntityBase?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
