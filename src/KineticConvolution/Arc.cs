using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public class Arc : Tracing
    {
        private Arc(
            Fraction weight,
            Point center,
            DirectionRange directions,
            IAlgebraicNumber radius,
            Point start,
            Point end,
            Direction startDirection,
            Direction endDirection)
            : base(start, end, startDirection, endDirection, weight)
        {
            Center = center ?? throw new ArgumentNullException(nameof(center));

            Directions = directions ?? throw new ArgumentNullException(nameof(directions));

            Radius = radius ?? throw new ArgumentNullException(nameof(radius));
        }

        public Point Center { get; }

        public DirectionRange Directions { get; }

        public IAlgebraicNumber Radius { get; }

        public static Arc Create(
            Fraction weight,
            Point center,
            DirectionRange directions,
            IAlgebraicNumber radius)
        {
            if (center == null)
            {
                throw new ArgumentNullException(nameof(center));
            }

            if (directions == null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            if (radius == null)
            {
                throw new ArgumentNullException(nameof(radius));
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

            return new Arc(weight, center, directions, radius, start, end, startDirection, endDirection);
        }
    }
}
