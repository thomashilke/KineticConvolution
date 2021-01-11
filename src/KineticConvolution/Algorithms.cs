using System;
using System.Collections.Generic;

namespace Hilke.KineticConvolution
{
    public static class Algorithms
    {
        public static Point Sum(Point point1, Point point2) =>
            new Point(point1.X.Add(point2.X), point1.Y.Add(point2.Y));

        public static IEnumerable<DirectionRange> Intersection(DirectionRange range1, DirectionRange range2)
        {
            var counterClockwiseRange1 =
                range1.Orientation == Orientation.CounterClockwise
                    ? range1
                    : range1.Reverse();

            var counterClockwiseRange2 =
                range2.Orientation == Orientation.CounterClockwise
                    ? range2
                    : range2.Reverse();

            return CounterClockwiseRangesIntersection(
                counterClockwiseRange1,
                counterClockwiseRange2);
        }

        public static int Compare(Direction reference, Direction direction1, Direction direction2)
        {
            if (direction1.Equals(direction2))
            {
                return 0;
            }

            return direction1.BelongsTo(
                       new DirectionRange(
                           reference,
                           direction2,
                           Orientation.CounterClockwise))
                       ? 1
                       : -1;
        }

        public static Direction FirstOfCounterClockwise(
            Direction reference,
            Direction direction1,
            Direction direction2) =>
            Compare(reference, direction1, direction2) switch
            {
                -1 => direction2,
                1 => direction1,
                0 => direction1,
                _ => throw new NotSupportedException() // TODO add a message
            };

        public static Direction LastOfCounterClockwise(
            Direction reference,
            Direction direction1,
            Direction direction2) =>
            Compare(reference, direction1, direction2) switch
            {
                -1 => direction1,
                1 => direction2,
                0 => direction1,
                _ => throw new NotSupportedException() // TODO add a message
            };

        private static IEnumerable<DirectionRange> CounterClockwiseRangesIntersection(
            DirectionRange range1,
            DirectionRange range2)
        {
            if (range1.Orientation != Orientation.CounterClockwise)
            {
                throw new ArgumentException(nameof(range1));
            }

            if (range2.Orientation != Orientation.CounterClockwise)
            {
                throw new ArgumentException(nameof(range2));
            }

            if (range2.Start.BelongsTo(range1))
            {
                yield return new DirectionRange(
                    range2.Start,
                    FirstOfCounterClockwise(
                        range1.Start,
                        range1.End,
                        range2.End),
                    Orientation.CounterClockwise);

                if (Compare(range1.End, range2.End, range1.Start) == -1)
                {
                    yield return new DirectionRange(
                        range1.Start,
                        range2.End,
                        Orientation.CounterClockwise);
                }
            }
            else if (range2.End.BelongsTo(range1))
            {
                yield return new DirectionRange(
                    range1.Start,
                    range2.End,
                    Orientation.CounterClockwise);
            }
        }
    }
}
