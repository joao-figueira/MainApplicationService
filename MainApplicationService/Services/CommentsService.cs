using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;
using MainApplicationService.Interfaces;
using MainApplicationService.Providers;

namespace MainApplicationService.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsValidationService _commentsValidationService;
        private readonly ICommentsRepository _commentsRepository;
        private readonly CurrentUserProvider _currentUserProvider;

        public CommentsService(ICommentsValidationService commentsValidationService,
            ICommentsRepository commentsRepository, CurrentUserProvider currentUserProvider)
        {
            _commentsValidationService = commentsValidationService;
            _commentsRepository = commentsRepository;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<ServiceOperationResult<Comment>> CreateAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            var validationResult = _commentsValidationService.ValidateCreate(comment);
            if (!validationResult.IsSuccess)
                return validationResult;
            comment.CreatedById = _currentUserProvider.CurrentUser?.Id;
            comment.CreatedOnUtc = DateTime.UtcNow;
            comment = await _commentsRepository.CreateAsync(comment, cancellationToken);
            return ServiceOperationResult<Comment>.Success(comment);
        }

        public async Task<ServiceOperationResult<Comment>> UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            var validationResult = _commentsValidationService.ValidateUpdate(comment);
            if (!validationResult.IsSuccess)
                return validationResult;
            comment.ModifiedById = _currentUserProvider.CurrentUser?.Id;
            comment.ModifiedOnUtc = DateTime.UtcNow;
            comment = await _commentsRepository.UpdateAsync(comment, cancellationToken);
            return ServiceOperationResult<Comment>.Success(comment);
        }

        public ServiceOperationResult<string> Delete(Comment comment, CancellationToken cancellationToken = default)
        {
            var deletedId = _commentsRepository.Delete(comment, cancellationToken);
            return ServiceOperationResult<string>.Success(deletedId ?? "");
        }

        public ServiceOperationResult<string> CheckGetPermissions(string? id = null)
        {
            var errorDictionary = new Dictionary<string, List<string>>();
            //ToDo: add business rules to check if the user has permissions, for example: roles or entity access
            return errorDictionary.Any()
                ? ServiceOperationResult<string>.Failure(errorDictionary)
                : ServiceOperationResult<string>.Success();
        }

        public ServiceOperationResult<string> CheckGetListPermissions(string? id = null)
        {
            var errorDictionary = new Dictionary<string, List<string>>();
            //ToDo: add business rules to check if the user has permissions, for example: roles or entity access
            return errorDictionary.Any()
                ? ServiceOperationResult<string>.Failure(errorDictionary)
                : ServiceOperationResult<string>.Success();
        }

        public ServiceOperationResult<string> CheckCreatePermissions()
        {
            var errorDictionary = new Dictionary<string, List<string>>();
            //ToDo: add business rules to check if the user has permissions, for example: roles or entity access
            return errorDictionary.Any()
                ? ServiceOperationResult<string>.Failure(errorDictionary)
                : ServiceOperationResult<string>.Success();
        }

        public ServiceOperationResult<string> CheckUpdatePermissions(string? id = null)
        {
            var errorDictionary = new Dictionary<string, List<string>>();
            //ToDo: add business rules to check if the user has permissions, for example: roles or entity access
            return errorDictionary.Any()
                ? ServiceOperationResult<string>.Failure(errorDictionary)
                : ServiceOperationResult<string>.Success();
        }

        public ServiceOperationResult<string> CheckDeletePermissions(string? id = null)
        {
            var errorDictionary = new Dictionary<string, List<string>>();
            //ToDo: add business rules to check if the user has permissions, for example: roles or entity access
            return errorDictionary.Any()
                ? ServiceOperationResult<string>.Failure(errorDictionary)
                : ServiceOperationResult<string>.Success();
        }
    }
}
