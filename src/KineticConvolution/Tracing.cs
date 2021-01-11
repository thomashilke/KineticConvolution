using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public abstract class Tracing
    {
        protected Tracing(Point start, Point end, Direction startDirection, Direction endDirection, Fraction weight)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));
            End = end ?? throw new ArgumentNullException(nameof(end));
            StartDirection = startDirection ?? throw new ArgumentNullException(nameof(startDirection));
            EndDirection = endDirection ?? throw new ArgumentNullException(nameof(endDirection));
            Weight = weight;
        }

        public Fraction Weight { get; }

        public Point Start { get; }

        public Point End { get; }

        public Direction StartDirection { get; }

        public Direction EndDirection { get; }

        public bool IsG1ContinuousWith(Tracing next) =>
            End == next.Start && EndDirection == next.StartDirection;
        
    }
}
