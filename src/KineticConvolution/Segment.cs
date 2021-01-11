using Fractions;

namespace Hilke.KineticConvolution
{
    public class Segment : Tracing
    {
        public Segment(Point start, Point end, Fraction weight)
            : base(start, end, start.DirectionTo(end), start.DirectionTo(end), weight) { } // TODO Find a better solution
    }
}
