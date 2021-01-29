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
            Direction<TAlgebraicNumber> startDirection,
            Direction<TAlgebraicNumber> endDirection,
            Fraction weight)
        {
            Calculator = calculator ?? throw new ArgumentNullException(nameof(start));
            Start = start ?? throw new ArgumentNullException(nameof(start));
            End = end ?? throw new ArgumentNullException(nameof(end));
            StartDirection = startDirection ?? throw new ArgumentNullException(nameof(startDirection));
            EndDirection = endDirection ?? throw new ArgumentNullException(nameof(endDirection));
            Weight = weight;
        }

        public Fraction Weight { get; }

        public Point<TAlgebraicNumber> Start { get; }

        public Point<TAlgebraicNumber> End { get; }

        public Direction<TAlgebraicNumber> StartDirection { get; }

        public Direction<TAlgebraicNumber> EndDirection { get; }

        protected IAlgebraicNumberCalculator<TAlgebraicNumber> Calculator { get; }

        public bool IsG1ContinuousWith(Tracing<TAlgebraicNumber> next) =>
            End == next.Start && EndDirection == next.StartDirection;
    }
}
