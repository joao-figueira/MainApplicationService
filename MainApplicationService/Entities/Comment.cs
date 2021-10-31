using System;

namespace MainApplicationService.Entities
{
    public class Comment : EntityBase
    {
        public string? ParentId { get; set; }
        public string? Text { get; set; }
        public string? CreatedById { get; set; }
        public string? ModifiedById { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }
}
