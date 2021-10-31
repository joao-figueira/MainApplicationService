using System;

namespace MainApplicationService.Entities
{
    public class CommentReadLog : EntityBase
    {
        public string? ParentEntityId { get; set; }
        public string? CommentId { get; set; }
        public string? UserId { get; set; }
        public DateTime? TimestampUtc { get; set; }
    }
}
