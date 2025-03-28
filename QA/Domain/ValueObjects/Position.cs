namespace QA.Domain.ValueObjects
{
    public class Position(int X, int Y)
    {
        public int X { get; } = X;
        public int Y { get; } = Y;
    }
}
