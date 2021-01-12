using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hilke.KineticConvolution
{
    public class DirectionRange<TAlgebraicNumber>
        where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        public DirectionRange(
            Direction<TAlgebraicNumber> start,
            Direction<TAlgebraicNumber> end,
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

        public Direction<TAlgebraicNumber> Start { get; }

        public Direction<TAlgebraicNumber> End { get; }

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
                var orientation => throw new NotSupportedException(
                    $"Only clockwise and counterclockwise arc orientations are supported, " +
                    $"but got {orientation}.")
            };

        public DirectionRange<TAlgebraicNumber> Reverse() =>
            new DirectionRange<TAlgebraicNumber>(
                End,
                Start,
                Orientation == Orientation.CounterClockwise
                    ? Orientation.Clockwise
                    : Orientation.CounterClockwise);

        public DirectionRange<TAlgebraicNumber> Opposite() =>
            new DirectionRange<TAlgebraicNumber>(
                Start.Opposite(),
                End.Opposite(),
                Orientation);

        public IEnumerable<DirectionRange<TAlgebraicNumber>> Intersection(DirectionRange<TAlgebraicNumber> other)
        {
            if (other == null)
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

            return counterClockwiseRange1.CounterClockwiseRangesIntersection(counterClockwiseRange2);
        }

        private IEnumerable<DirectionRange<TAlgebraicNumber>> CounterClockwiseRangesIntersection(DirectionRange<TAlgebraicNumber> range)
        {
            if (Orientation != Orientation.CounterClockwise)
            {
                throw new InvalidOperationException("The direction range must be counterclockwise.");
            }

            if (range.Orientation != Orientation.CounterClockwise)
            {
                throw new ArgumentException(
                    "The direction range must be counterclockwise.", nameof(range));
            }

            if (range.Start.BelongsTo(this))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    range.Start,
                        Start.FirstOf(End, range.End),
                    Orientation.CounterClockwise);

                if (End.CompareTo(range.End, Start) == -1)
                {
                    yield return new DirectionRange<TAlgebraicNumber>(
                        Start,
                        range.End,
                        Orientation.CounterClockwise);
                }
            }
            else if (range.End.BelongsTo(this))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    Start,
                    range.End,
                    Orientation.CounterClockwise);
            }
        }
    }
}
