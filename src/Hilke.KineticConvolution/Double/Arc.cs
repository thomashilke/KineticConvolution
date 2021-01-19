using System;

using Fractions;

namespace Hilke.KineticConvolution.Double
{
    public class Arc : Tracing
    {
        internal Arc(
            Fraction weight,
            Point center,
            DirectionRange directions,
            double radius,
            Point start,
            Point end,
            Direction startDirection,
            Direction endDirection)
            : base(start, end, startDirection, endDirection, weight)
        {
            Center = center ?? throw new ArgumentNullException(nameof(center));
            Directions = directions ?? throw new ArgumentNullException(nameof(directions));
        }

        public Point Center { get; }

        public DirectionRange Directions { get; }

        public double Radius { get; }
    }
}
