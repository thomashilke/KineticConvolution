using System;

using Fractions;

namespace Hilke.KineticConvolution.Double
{
    public class Arc<TAlgebraicNumber> : Tracing<TAlgebraicNumber>
        where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        internal Arc(
            Fraction weight,
            Point<TAlgebraicNumber> center,
            DirectionRange<TAlgebraicNumber> directions,
            TAlgebraicNumber radius,
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Direction<TAlgebraicNumber> startDirection,
            Direction<TAlgebraicNumber> endDirection)
            : base(start, end, startDirection, endDirection, weight)
        {
            Center = center ?? throw new ArgumentNullException(nameof(center));
            Directions = directions ?? throw new ArgumentNullException(nameof(directions));
            Radius = radius ?? throw new ArgumentNullException(nameof(radius));
        }

        public Point<TAlgebraicNumber> Center { get; }

        public DirectionRange<TAlgebraicNumber> Directions { get; }

        public TAlgebraicNumber Radius { get; }
    }
}
