namespace Hilke.KineticConvolution
{
    public static class SegmentExtensions
    {
        public static Direction Direction(this Segment segment) =>
            new Direction(
                segment.End.X.Subtract(segment.Start.X),
                segment.End.Y.Subtract(segment.Start.Y));

        public static Direction NormalDirection(this Segment segment)
        {
            var direction = segment.Direction();
            return new Direction(direction.Y.Opposite(), direction.X);
        }
    }
}
