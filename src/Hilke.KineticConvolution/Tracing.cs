using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public abstract class Tracing<TAlgebraicNumber>
    {
        protected Tracing(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Direction<TAlgebraicNumber> startTangentDirection,
            Direction<TAlgebraicNumber> endTangentDirection,
            Fraction weight)
        {
            Calculator = calculator ?? throw new ArgumentNullException(nameof(start));
            Start = start ?? throw new ArgumentNullException(nameof(start));
            End = end ?? throw new ArgumentNullException(nameof(end));
            StartTangentDirection = startTangentDirection ?? throw new ArgumentNullException(nameof(startTangentDirection));
            EndTangentDirection = endTangentDirection ?? throw new ArgumentNullException(nameof(endTangentDirection));
            Weight = weight;
        }

        public Fraction Weight { get; }

        public Point<TAlgebraicNumber> Start { get; }

        public Point<TAlgebraicNumber> End { get; }

        public Direction<TAlgebraicNumber> StartTangentDirection { get; }

        public Direction<TAlgebraicNumber> EndTangentDirection { get; }

        protected IAlgebraicNumberCalculator<TAlgebraicNumber> Calculator { get; }

        public bool IsContinuousWith(Tracing<TAlgebraicNumber> next) =>
            End == next.Start;

        public bool IsTangentContinuousWith(Tracing<TAlgebraicNumber> next) =>
            EndTangentDirection.Normalize() == next.StartTangentDirection.Normalize();
    }
}
