namespace MainApplicationService.Entities
{
    public class EntityBase
    {
        public string? Id { get; set; }
    }

    public enum EntityType
    {
        Comment,
        Article
    }
}
