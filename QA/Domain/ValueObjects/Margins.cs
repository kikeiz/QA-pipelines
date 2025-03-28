namespace QA.Domain.ValueObjects
{
    public class Margins(int? Top, int? Left, int? Bottom, int? Right)
    {
        // Properties are automatically inferred
        public int? Top { get; } = Top;
        public int? Left { get; } = Left;
        public int? Bottom { get; } = Bottom;
        public int? Right { get; } = Right;
    }
}
