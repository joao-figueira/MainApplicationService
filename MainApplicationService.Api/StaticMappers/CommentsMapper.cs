using MainApplicationService.Api.Dtos;
using MainApplicationService.Entities;

namespace MainApplicationService.Api.StaticMappers
{
    public static class CommentsMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new()
            {
                Id = comment.Id,
                ParentId = comment.ParentId,
                Text = comment.Text,
                CreatedById = comment.CreatedById,
                CreatedOnUtc = comment.CreatedOnUtc,
                ModifiedById = comment.ModifiedById,
                ModifiedOnUtc = comment.ModifiedOnUtc
            };
        }
    }
}
