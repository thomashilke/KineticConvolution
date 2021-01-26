using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public class Segment<TAlgebraicNumber> : Tracing<TAlgebraicNumber>
        where TAlgebraicNumber : IEquatable<TAlgebraicNumber>
    {
        internal Segment(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Direction<TAlgebraicNumber> startDirection,
            Direction<TAlgebraicNumber> endDirection,
            Fraction weight)
            : base(calculator, start, end, startDirection, endDirection, weight) { }

        public Direction<TAlgebraicNumber> Direction() =>
            new Direction<TAlgebraicNumber>(
                Calculator,
                Calculator.Subtract(End.X, Start.X),
                Calculator.Subtract(End.Y, Start.Y));

        public Direction<TAlgebraicNumber> NormalDirection()
        {
            var direction = Direction();
            return new Direction<TAlgebraicNumber>(
                Calculator,
                Calculator.Opposite(direction.Y),
                direction.X);
        }
    }
}
