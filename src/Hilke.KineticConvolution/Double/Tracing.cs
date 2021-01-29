using System;

using Fractions;

namespace Hilke.KineticConvolution.Double
{
    public abstract class Tracing
    {
        protected Tracing(
            Point start,
            Point end,
            Direction startDirection,
            Direction endDirection,
            Fraction weight)
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

        public bool IsG1ContinuousWith(Tracing next) => End == next.Start && EndDirection == next.StartDirection;

        public static Tracing CreateArc(
            Fraction weight,
            Point center,
            DirectionRange directions,
            double radius)
        {
            if (center is null)
            {
                throw new ArgumentNullException(nameof(center));
            }

            if (directions is null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            var start = center.Translate(directions.Start, radius);
            var end = center.Translate(directions.End, radius);

            var startNormalDirection = directions.Start.NormalDirection();
            var startDirection = directions.Orientation == Orientation.Clockwise
                                     ? startNormalDirection.Opposite()
                                     : startNormalDirection;

            var endNormalDirection = directions.End.NormalDirection();
            var endDirection = directions.Orientation == Orientation.Clockwise
                                   ? endNormalDirection.Opposite()
                                   : endNormalDirection;

            return new Arc(
                weight,
                center,
                directions,
                radius,
                start,
                end,
                startDirection,
                endDirection);
        }

        public static Tracing CreateSegment(
            Point start,
            Point end,
            Fraction weight)
        {
            if (start is null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (end is null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            if (start == end)
            {
                throw new ArgumentException("The start point cannot be the same as end point.", nameof(start));
            }

            var direction = start.DirectionTo(end);
            return new Segment(start, end, direction, direction, weight);
        }
    }
}
