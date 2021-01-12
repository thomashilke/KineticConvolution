using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public class Arc : Tracing
    {
        internal Arc(
            Fraction weight,
            Point center,
            DirectionRange directions,
            IAlgebraicNumber radius,
            Point start,
            Point end,
            Direction startDirection,
            Direction endDirection)
            : base(start, end, startDirection, endDirection, weight)
        {
            Center = center ?? throw new ArgumentNullException(nameof(center));
            Directions = directions ?? throw new ArgumentNullException(nameof(directions));
            Radius = radius ?? throw new ArgumentNullException(nameof(radius));
        }

        public Point Center { get; }

        public DirectionRange Directions { get; }

        public IAlgebraicNumber Radius { get; }
    }
}
