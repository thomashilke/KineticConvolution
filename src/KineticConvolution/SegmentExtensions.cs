using System;

namespace KineticConvolution
{
    public static class SegmentExtensions
    {
        public static Direction Direction(this Segment segment)
        {
            return new Direction(
                segment.End.X.Substract(segment.Start.X),
                segment.End.Y.Substract(segment.Start.Y));
        }

        public static Direction NormalDirection(this Segment segment)
        {
            var direction = segment.Direction();
            return new Direction(direction.Y.Opposite(), direction.X);
        }
    }
}
