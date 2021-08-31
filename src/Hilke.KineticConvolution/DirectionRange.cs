using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Hilke.KineticConvolution
{
    [DebuggerDisplay("DirectionRange(Start: {Start}, End: {End}, Orientation: {Orientation})")]
    public sealed class DirectionRange<TAlgebraicNumber>
    {
        private readonly IAlgebraicNumberCalculator<TAlgebraicNumber> _calculator;

        internal DirectionRange(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            Direction<TAlgebraicNumber> start,
            Direction<TAlgebraicNumber> end,
            Orientation orientation)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));

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

        internal bool IsDegenerate() => Start == End;

        internal bool IsShortestRange()
        {
            if (IsDegenerate())
            {
                return false;
            }

            return Orientation switch
            {
                Orientation.Clockwise =>
                    _calculator.IsNegative(Start.Determinant(End)),

                Orientation.CounterClockwise =>
                    _calculator.IsStrictlyPositive(Start.Determinant(End)),

                var orientation => throw new NotSupportedException(
                        "Only clockwise and counterclockwise arc orientations are supported, "
                        + $"but got {orientation}.")
            };
        }

        public DirectionRange<TAlgebraicNumber> Reverse() =>
            new DirectionRange<TAlgebraicNumber>(
                _calculator,
                End,
                Start,
                Orientation == Orientation.CounterClockwise
                    ? Orientation.Clockwise
                    : Orientation.CounterClockwise);

        public DirectionRange<TAlgebraicNumber> Opposite() =>
            new DirectionRange<TAlgebraicNumber>(
                _calculator,
                Start.Opposite(),
                End.Opposite(),
                Orientation);

        public IEnumerable<DirectionRange<TAlgebraicNumber>> Intersection(DirectionRange<TAlgebraicNumber> other)
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

            var intersections =
                CounterClockwiseRangesIntersection(
                    _calculator,
                    counterClockwiseRange1,
                    counterClockwiseRange2)
                .ToList();

            return Orientation == Orientation.CounterClockwise
                    ? intersections
                    : intersections.Select(t => t.Reverse());
        }

        public IEnumerable<DirectionRange<TAlgebraicNumber>> Union(
            DirectionRange<TAlgebraicNumber> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var counterClockwiseRange1 =
                this.Orientation == Orientation.CounterClockwise
                    ? this
                    : this.Reverse();

            var counterClockwiseRange2 =
                other.Orientation == Orientation.CounterClockwise
                    ? other
                    : other.Reverse();

            var unions =
                CounterClockwiseRangeUnion(
                    _calculator,
                    counterClockwiseRange1,
                    counterClockwiseRange2)
                .ToList();

            return this.Orientation == Orientation.CounterClockwise
               ? unions
               : unions.Select(t => t.Reverse());
        }

        private static IEnumerable<DirectionRange<TAlgebraicNumber>> CounterClockwiseRangesIntersection(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            DirectionRange<TAlgebraicNumber> range1,
            DirectionRange<TAlgebraicNumber> range2)
        {
            if (range1.IsDegenerate())
            {
                return CounterClockwiseDegenerateRangesIntersection(
                    calculator, range2, range1);
            }

            if (range2.IsDegenerate())
            {
               return CounterClockwiseDegenerateRangesIntersection(
                   calculator, range1, range2);
            }

            return CounterClockwiseRegularRangesIntersection(calculator, range1, range2);
        }

        private static IEnumerable<DirectionRange<TAlgebraicNumber>> CounterClockwiseDegenerateRangesIntersection(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            DirectionRange<TAlgebraicNumber> range,
            DirectionRange<TAlgebraicNumber> degenerateRange)
        {
            if (!degenerateRange.IsDegenerate())
            {
                throw new InvalidOperationException("A degenerated range is expected.");
            }

            if (degenerateRange.Start.StrictlyBelongsTo(range))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    calculator,
                    degenerateRange.Start, range.End,
                    Orientation.CounterClockwise);

                yield return new DirectionRange<TAlgebraicNumber>(
                    calculator,
                    range.Start, degenerateRange.Start,
                    Orientation.CounterClockwise);
            }
            else
            {
                yield return range;
            }
        }

        private static IEnumerable<DirectionRange<TAlgebraicNumber>> CounterClockwiseRegularRangesIntersection(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            DirectionRange<TAlgebraicNumber> range1,
            DirectionRange<TAlgebraicNumber> range2)
        {
            if (range1.Orientation != Orientation.CounterClockwise)
            {
                throw new InvalidOperationException("The direction range must be counterclockwise.");
            }

            if (range2.Orientation != Orientation.CounterClockwise)
            {
                throw new ArgumentException("The direction range must be counterclockwise.", nameof(range2));
            }

            if (range2.Start.StrictlyBelongsTo(range1))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    calculator,
                    range2.Start,
                    range2.Start.FirstOf(range1.End, range2.End),
                    Orientation.CounterClockwise);

                if (range2.Start.CompareTo(range2.End, range1.Start) == DirectionOrder.After
                    && range2.End.CompareTo(range1.Start, range1.End) == DirectionOrder.After)
                {
                    yield return new DirectionRange<TAlgebraicNumber>(
                        calculator,
                        range1.Start,
                        range2.End,
                        Orientation.CounterClockwise);
                }
            }
            else if (range2.End.CompareTo(range1.Start, range2.Start) == DirectionOrder.After)
            {
                if (range1.Start == range1.End)
                {
                    yield return new DirectionRange<TAlgebraicNumber>(
                        calculator,
                        range1.Start,
                        range2.End,
                        Orientation.CounterClockwise);
                }
                else
                {
                    yield return new DirectionRange<TAlgebraicNumber>(
                        calculator,
                        range1.Start,
                        range1.Start.FirstOf(range1.End, range2.End),
                        Orientation.CounterClockwise);
                }
            }
        }

        private static IEnumerable<DirectionRange<TAlgebraicNumber>> CounterClockwiseRangeUnion(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            DirectionRange<TAlgebraicNumber> range1,
            DirectionRange<TAlgebraicNumber> range2)
        {
            if (range1.Orientation != Orientation.CounterClockwise ||
                range2.Orientation != Orientation.CounterClockwise)
            {
                throw new InvalidOperationException(
                    "The direction range must be counterclockwise.");
            }

            if(range1.Start == range2.Start && range1.End == range2.End)
            {
                yield return range1;
            }
            else if (range2.Start.BelongsTo(range1) && !range2.End.BelongsTo(range1))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    calculator,
                    range1.Start,
                    range2.End,
                    Orientation.CounterClockwise);
            }
            else if (range1.Start.BelongsTo(range2) && !range1.End.BelongsTo(range2))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    calculator,
                    range2.Start,
                    range1.End,
                    Orientation.CounterClockwise);
            }
            else if (range2.Start.BelongsTo(range1)
                      && range2.End.BelongsTo(range1)
                      && range1.Start.BelongsTo(range2)
                      && range1.End.BelongsTo(range2))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    calculator,
                    range1.Start,
                    range1.Start,
                    Orientation.CounterClockwise);
            }
            else if (range2.Start.BelongsTo(range1) && range2.End.BelongsTo(range1))
            {
                yield return range1;
            }
            else if (range1.Start.BelongsTo(range2) && range1.End.BelongsTo(range2))
            {
                yield return range2;
            }
            else
            {
                yield return range1;
                yield return range2;
            }
        }
    }
}
