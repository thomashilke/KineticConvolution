using System;
using System.Diagnostics;

using Fractions;

namespace Hilke.KineticConvolution
{
    [DebuggerDisplay("Arc(Center: {Center}, Directions: {Directions}, Radius: {Radius}, Weight: {Weight})")]
    public sealed class Arc<TAlgebraicNumber> : Tracing<TAlgebraicNumber>
    {
        internal Arc(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            Fraction weight,
            Point<TAlgebraicNumber> center,
            DirectionRange<TAlgebraicNumber> directions,
            TAlgebraicNumber radius,
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Direction<TAlgebraicNumber> startDirection,
            Direction<TAlgebraicNumber> endDirection)
            : base(calculator, start, end, startDirection, endDirection, weight)
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
