using Fractions;

using System;

namespace KineticConvolution
{
    public class Segment : Tracing
    {
        public Point End { get; }

        public Point Start { get; }

        public Segment(Fraction weight, Point start, Point end)
            : base(weight)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));

            End = end ?? throw new ArgumentNullException(nameof(end));
        }
    }
}
