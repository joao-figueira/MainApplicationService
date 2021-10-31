using System;

namespace MainApplicationService.Entities
{
    public class Article : EntityBase
    {
        public string? Title { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
    }
}
