#nullable enable
using System.Collections.Generic;

namespace MainApplicationService.Services
{
    public class ServiceOperationResult<T> where T : class
    {
        public bool IsSuccess { get; }
        public T? ResultObject { get; }
        public IReadOnlyDictionary<string, List<string>>? ValidationErrors { get; }

        private ServiceOperationResult()
        {
            IsSuccess = true;
        }

        private ServiceOperationResult(T resultObject)
        {
            IsSuccess = true;
            ResultObject = resultObject;
        }

        private ServiceOperationResult(IReadOnlyDictionary<string, List<string>> validationErrors)
        {
            IsSuccess = false;
            ValidationErrors = validationErrors;
        }

        public static ServiceOperationResult<T> Success(T resultObject)
        {
            return new ServiceOperationResult<T>(resultObject);
        }
        public static ServiceOperationResult<T> Success()
        {
            return new ServiceOperationResult<T>();
        }

        public static ServiceOperationResult<T> Failure(IReadOnlyDictionary<string, List<string>> validationErrors)
        {
            return new ServiceOperationResult<T>(validationErrors);
        }
    }
}
