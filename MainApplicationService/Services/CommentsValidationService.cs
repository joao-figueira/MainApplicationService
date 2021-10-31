using System.Collections.Generic;
using System.Linq;
using MainApplicationService.Entities;
using MainApplicationService.Helpers;
using MainApplicationService.Interfaces;

namespace MainApplicationService.Services
{
    public class CommentsValidationService : ICommentsValidationService
    {
        public ServiceOperationResult<Comment> ValidateCreate(Comment comment)
        {
            var validationErrors = new Dictionary<string, List<string>>();
            if (string.IsNullOrEmpty(comment.Text))
            {
                ErrorsDictionaryHelper.AddToDictionary(validationErrors, "Text", "Text should not be null or empty.");
            } 
            else if (OffensiveContentHelper.ContainsOffensiveExpressions(comment.Text, out var offensiveExpressionsFound))
            {
                var expressions = string.Join(";", offensiveExpressionsFound);
                ErrorsDictionaryHelper.AddToDictionary(validationErrors, "Text", $"Offensive expressions were found on the comment's text: {expressions}");
            }

            return validationErrors.Any() ? ServiceOperationResult<Comment>.Failure(validationErrors) : ServiceOperationResult<Comment>.Success(comment);
        }

        public ServiceOperationResult<Comment> ValidateUpdate(Comment comment)
        {
            var validationErrors = new Dictionary<string, List<string>>();
            if (string.IsNullOrEmpty(comment.Text))
            {
                ErrorsDictionaryHelper.AddToDictionary(validationErrors, "Text", "Text should not be null or empty.");
            }
            else if (OffensiveContentHelper.ContainsOffensiveExpressions(comment.Text, out var offensiveExpressionsFound))
            {
                var expressions = string.Join(";", offensiveExpressionsFound);
                ErrorsDictionaryHelper.AddToDictionary(validationErrors, "Text", $"Offensive expressions were found on the comment's text: {expressions}");
            }

            return validationErrors.Any() ? ServiceOperationResult<Comment>.Failure(validationErrors) : ServiceOperationResult<Comment>.Success(comment);
        }
    }
}
