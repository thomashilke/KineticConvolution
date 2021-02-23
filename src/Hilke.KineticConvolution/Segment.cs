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
            Direction<TAlgebraicNumber> startTangentDirection,
            Direction<TAlgebraicNumber> endTangentDirection,
            Fraction weight)
            : base(calculator, start, end, startTangentDirection, endTangentDirection, weight)
        {
            NormalDirection = new Direction<TAlgebraicNumber>(
                Calculator,
                Calculator.Opposite(startTangentDirection.Y),
                startTangentDirection.X);
        }

        public Direction<TAlgebraicNumber> Direction => StartTangentDirection;

        public Direction<TAlgebraicNumber> NormalDirection { get; }
    }
}
