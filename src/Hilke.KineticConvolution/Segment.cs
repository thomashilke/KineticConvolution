using System.Diagnostics;

using Fractions;

namespace Hilke.KineticConvolution
{
    [DebuggerDisplay("Segment(Start: {Start}, End: {End}, Direction: {StartDirection}, Weight: {Weight})")]
    public sealed class Segment<TAlgebraicNumber> : Tracing<TAlgebraicNumber>
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
