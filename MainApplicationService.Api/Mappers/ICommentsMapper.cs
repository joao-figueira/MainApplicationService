using MainApplicationService.Api.Dtos;
using MainApplicationService.Entities;

namespace MainApplicationService.Api.Mappers
{
    public interface ICommentsMapper
    {
        public Comment Create(string parentId, CommentPostModel postModel);
        public Comment Update(Comment comment, CommentPutModel putModel);
    }
}
