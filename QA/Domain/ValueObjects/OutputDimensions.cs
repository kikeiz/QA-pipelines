namespace QA.Domain.ValueObjects
{
    public class OutputDimensions(int? OutputHeight, int? OutputWidth)
    {
        public int? OutputHeight { get; set;} = OutputHeight;
        public int? OutputWidth { get; set;} = OutputWidth;
    }
}
