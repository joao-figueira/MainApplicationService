using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;
using MainApplicationService.Services;

namespace MainApplicationService.Interfaces
{
    public interface ICommentsService : IPermissionsService
    {
        public Task<ServiceOperationResult<Comment>> CreateAsync(Comment comment, CancellationToken cancellationToken = default);
        public Task<ServiceOperationResult<Comment>> UpdateAsync(Comment comment, CancellationToken cancellationToken = default);
        public ServiceOperationResult<string> Delete(Comment comment, CancellationToken cancellationToken = default);
    }
}
