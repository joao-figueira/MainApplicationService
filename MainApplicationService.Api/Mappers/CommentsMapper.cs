using MainApplicationService.Api.Dtos;
using MainApplicationService.Entities;

namespace MainApplicationService.Api.Mappers
{
    public class CommentsMapper : ICommentsMapper
    {
        public Comment Create(string parentId, CommentPostModel postModel)
        {
            return new()
            {
                ParentId = parentId,
                Text = postModel.Text
            };
        }

        public Comment Update(Comment comment, CommentPutModel putModel)
        {
            comment.Text = putModel.Text;
            return comment;
        }
    }
}
