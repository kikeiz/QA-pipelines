namespace QA.Domain.ValueObjects
{
    public class BaseDimensions(int Width, int Height)
    {
        public int Width { get; } = Width;
        public int Height { get; } = Height;
    }
}
