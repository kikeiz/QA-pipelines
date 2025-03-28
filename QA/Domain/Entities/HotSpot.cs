namespace QA.Domain.Entities
{
    using QA.Domain.ValueObjects;
    public class HotSpot(
        string id,
        int x, 
        int y, 
        int width, 
        int height)
    {
        public string Id { get; } = id;
        public Position Position { get; } = new(x, y);
        public BaseDimensions Base { get; } = new(width, height);
    }
}
