using System;
using System.Collections.Generic;

namespace MainApplicationService.Api.Dtos
{
    public class CommentDto
    {
        public string? Id { get; set; }
        public string? ParentEntityId { get; set; }
        public string? Text { get; set; }
        public string? CreatedById { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }

    public class CommentsListDto
    {
        public IEnumerable<CommentDto>? Results { get; set; }
        public int TotalCount { get; set; }
    }

    public class CommentPostModel
    {
        public string? Text { get; set; }
    }
    
    public class CommentPutModel
    {
        public string? Text { get; set; }
    }
}
