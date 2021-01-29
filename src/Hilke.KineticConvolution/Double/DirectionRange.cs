using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Hilke.KineticConvolution.Double
{
    public class DirectionRange
    {
        public DirectionRange(
            Direction start,
            Direction end,
            Orientation orientation)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));

            End = end ?? throw new ArgumentNullException(nameof(end));

            if (!Enum.IsDefined(typeof(Orientation), orientation))
            {
                throw new InvalidEnumArgumentException(
                    nameof(orientation),
                    (int)orientation,
                    typeof(Orientation));
            }

            Orientation = orientation;
        }

        public Direction Start { get; }

        public Direction End { get; }

        public Orientation Orientation { get; }

        public bool IsShortestRange() =>
            Orientation switch
            {
                Orientation.Clockwise =>
                    DirectionHelper.Determinant(Start, End) < 0.0,
                Orientation.CounterClockwise =>
                    DirectionHelper.Determinant(Start, End) > 0.0,
                var orientation => throw new NotSupportedException(
                                       "Only clockwise and counterclockwise arc orientations are supported, "
                                     + $"but got {orientation}.")
            };

        public DirectionRange Reverse() =>
            new DirectionRange(
                End,
                Start,
                Orientation == Orientation.CounterClockwise
                    ? Orientation.Clockwise
                    : Orientation.CounterClockwise);

        public DirectionRange Opposite() =>
            new DirectionRange(
                Start.Opposite(),
                End.Opposite(),
                Orientation);

        public IEnumerable<DirectionRange> Intersection(DirectionRange other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var counterClockwiseRange1 =
                Orientation == Orientation.CounterClockwise
                    ? this
                    : Reverse();

            var counterClockwiseRange2 =
                other.Orientation == Orientation.CounterClockwise
                    ? other
                    : other.Reverse();

            return counterClockwiseRange1.CounterClockwiseRangesIntersection(counterClockwiseRange2).ToList();
        }

        private IEnumerable<DirectionRange> CounterClockwiseRangesIntersection(DirectionRange range)
        {
            if (Orientation != Orientation.CounterClockwise)
            {
                throw new InvalidOperationException("The direction range must be counterclockwise.");
            }

            if (range.Orientation != Orientation.CounterClockwise)
            {
                throw new ArgumentException("The direction range must be counterclockwise.", nameof(range));
            }

            if (range.Start.BelongsTo(this))
            {
                yield return new DirectionRange(
                    range.Start,
                    Start.FirstOf(End, range.End),
                    Orientation.CounterClockwise);

                if (Start.CompareTo(range.Start, range.End) == DirectionOrder.Before
                 && End.CompareTo(range.End, Start) == DirectionOrder.Before)
                {
                    yield return new DirectionRange(
                        Start,
                        range.End,
                        Orientation.CounterClockwise);
                }
            }
            else if (range.Start.CompareTo(range.End, Start) == DirectionOrder.Before)
            {
                yield return new DirectionRange(
                    Start,
                    Start.FirstOf(End, range.End),
                    Orientation.CounterClockwise);
            }
        }
    }
}
