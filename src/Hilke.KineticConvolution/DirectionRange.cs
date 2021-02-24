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

        internal bool IsShortestRange()
        {
            if (Start == End)
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

            var result = counterClockwiseRange1.CounterClockwiseRangesIntersection(counterClockwiseRange2).ToList();

            return Orientation == Orientation.CounterClockwise
                    ? result
                    : result.Select(t => t.Reverse());
        }

        private IEnumerable<DirectionRange<TAlgebraicNumber>> CounterClockwiseRangesIntersection(
            DirectionRange<TAlgebraicNumber> range)
        {
            if (Orientation != Orientation.CounterClockwise)
            {
                throw new InvalidOperationException("The direction range must be counterclockwise.");
            }

            if (range.Orientation != Orientation.CounterClockwise)
            {
                throw new ArgumentException("The direction range must be counterclockwise.", nameof(range));
            }

            if (range.Start.StrictlyBelongsTo(this))
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    _calculator,
                    range.Start,
                    range.Start.FirstOf(End, range.End),
                    Orientation.CounterClockwise);

                if (Start.CompareTo(range.Start, range.End) == DirectionOrder.After
                 && End.CompareTo(range.End, Start) == DirectionOrder.After)
                {
                    yield return new DirectionRange<TAlgebraicNumber>(
                        _calculator,
                        Start,
                        range.End,
                        Orientation.CounterClockwise);
                }
            }
            else if (range.Start.CompareTo(range.End, Start) == DirectionOrder.After)
            {
                yield return new DirectionRange<TAlgebraicNumber>(
                    _calculator,
                    Start,
                    Start.FirstOf(End, range.End),
                    Orientation.CounterClockwise);
            }
        }
    }
}
