using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public class Segment : Tracing
    {
        public Segment(Fraction weight, Point start, Point end)
            : base(weight)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));

            End = end ?? throw new ArgumentNullException(nameof(end));
        }

        public Point End { get; }

        public Point Start { get; }
    }
}
