using MainApplicationService.Services;

namespace MainApplicationService.Interfaces
{
    public interface IPermissionsService
    {
        public ServiceOperationResult<string> CheckGetPermissions(string? id = null);
        public ServiceOperationResult<string> CheckGetListPermissions(string? id = null);
        public ServiceOperationResult<string> CheckCreatePermissions();
        public ServiceOperationResult<string> CheckUpdatePermissions(string? id = null);
        public ServiceOperationResult<string> CheckDeletePermissions(string? id = null);
    }
}
