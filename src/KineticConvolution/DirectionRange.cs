using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hilke.KineticConvolution
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
                    DirectionHelper.Determinant(Start, End)
                                   .IsStrictlyNegative(),
                Orientation.CounterClockwise =>
                    DirectionHelper.Determinant(Start, End)
                                   .IsStrictlyPositive(),
                _ => throw new NotSupportedException() // TODO add a message
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

        public IEnumerable<DirectionRange> Intersection(DirectionRange range2)
        {
            var counterClockwiseRange1 =
                Orientation == Orientation.CounterClockwise
                    ? this
                    : Reverse();

            var counterClockwiseRange2 =
                range2.Orientation == Orientation.CounterClockwise
                    ? range2
                    : range2.Reverse();

            return counterClockwiseRange1.CounterClockwiseRangesIntersection(counterClockwiseRange2);
        }

        private IEnumerable<DirectionRange> CounterClockwiseRangesIntersection(DirectionRange range2)
        {
            if (Orientation != Orientation.CounterClockwise)
            {
                throw new InvalidOperationException("The direction range must be counter clockwise.");
            }

            if (range2.Orientation != Orientation.CounterClockwise)
            {
                throw new ArgumentException(nameof(range2));
            }

            if (range2.Start.BelongsTo(this))
            {
                yield return new DirectionRange(
                    range2.Start,
                        Start.FirstOfCounterClockwise(End, range2.End),
                    Orientation.CounterClockwise);

                if (End.Compare(range2.End, Start) == -1)
                {
                    yield return new DirectionRange(
                        Start,
                        range2.End,
                        Orientation.CounterClockwise);
                }
            }
            else if (range2.End.BelongsTo(this))
            {
                yield return new DirectionRange(
                    Start,
                    range2.End,
                    Orientation.CounterClockwise);
            }
        }
    }
}
