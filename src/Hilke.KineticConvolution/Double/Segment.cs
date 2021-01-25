using Fractions;

namespace Hilke.KineticConvolution.Double
{
    public class Segment : Tracing
    {
        internal Segment(
            Point start,
            Point end,
            Direction startDirection,
            Direction endDirection,
            Fraction weight)
            : base(start, end, startDirection, endDirection, weight) { }

        public Direction Direction() =>
            new Direction(
                End.X - Start.X,
                End.Y - Start.Y);

        public Direction NormalDirection()
        {
            var direction = Direction();
            return new Direction(-direction.Y, direction.X);
        }
    }
}
