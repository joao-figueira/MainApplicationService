using MainApplicationService.Entities;
using MainApplicationService.Services;

namespace MainApplicationService.Interfaces
{
    public interface ICommentsValidationService
    {
        public ServiceOperationResult<Comment> ValidateCreate(Comment comment);
        public ServiceOperationResult<Comment> ValidateUpdate(Comment comment);
    }
}
