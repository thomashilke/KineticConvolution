using Fractions;

namespace Hilke.KineticConvolution
{
    public class Segment : Tracing
    {
        internal Segment(Point start, Point end, Direction startDirection, Direction endDirection, Fraction weight)
            : base(start, end, startDirection, endDirection, weight) { }
    }
}
