using System;

namespace Hilke.KineticConvolution
{
    public sealed class Direction<TAlgebraicNumber> : IEquatable<Direction<TAlgebraicNumber>>
    {
        private readonly IAlgebraicNumberCalculator<TAlgebraicNumber> _calculator;

        internal Direction(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber x,
            TAlgebraicNumber y)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (calculator.IsZero(x) && calculator.IsZero(y))
            {
                throw new ArgumentException(
                    "Both components of the direction cannot be simultaneously zero.",
                    nameof(y));
            }

            _calculator = calculator;

            X = x;
            Y = y;
        }

        public TAlgebraicNumber X { get; }

        public TAlgebraicNumber Y { get; }

        /// <inheritdoc />
        public bool Equals(Direction<TAlgebraicNumber>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return _calculator.IsZero(Determinant(other))
                && _calculator.Sign(X) == _calculator.Sign(other.X)
                && _calculator.Sign(Y) == _calculator.Sign(other.Y);
        }

        public TAlgebraicNumber Determinant(Direction<TAlgebraicNumber> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var a = _calculator.Multiply(X, other.Y);
            var b = _calculator.Multiply(Y, other.X);
            return _calculator.Subtract(a, b);
        }

        public DirectionOrder CompareTo(Direction<TAlgebraicNumber> direction1, Direction<TAlgebraicNumber> direction2)
        {
            if (direction1.Equals(direction2))
            {
                return DirectionOrder.Equal;
            }

            return direction1.BelongsTo(
                       new DirectionRange<TAlgebraicNumber>(
                           _calculator,
                           this,
                           direction2,
                           Orientation.CounterClockwise))
                       ? DirectionOrder.After
                       : DirectionOrder.Before;
        }

        public Direction<TAlgebraicNumber> FirstOf(
            Direction<TAlgebraicNumber> direction1,
            Direction<TAlgebraicNumber> direction2) =>
            CompareTo(direction1, direction2) switch
            {
                DirectionOrder.Before => direction2,
                DirectionOrder.After => direction1,
                DirectionOrder.Equal => direction1,
                var order =>
                    throw new NotSupportedException(
                        $"Comparison between two directions should yield either {DirectionOrder.Before}, "
                      + $"{DirectionOrder.Equal} or {DirectionOrder.After}, but got {(int)order}.")
            };

        public Direction<TAlgebraicNumber> LastOf(
            Direction<TAlgebraicNumber> direction1,
            Direction<TAlgebraicNumber> direction2) =>
            CompareTo(direction1, direction2) switch
            {
                DirectionOrder.Before => direction1,
                DirectionOrder.After => direction2,
                DirectionOrder.Equal => direction1,
                var order =>
                    throw new NotSupportedException(
                        $"Comparison between two directions should yield either {DirectionOrder.Before}, "
                      + $"{DirectionOrder.Equal} or {DirectionOrder.After}, but got {(int)order}.")
            };

        public bool BelongsToShortestRange(DirectionRange<TAlgebraicNumber> directions)
        {
            var determinant = directions.Start.Determinant(directions.End);

            if (_calculator.IsStrictlyPositive(determinant))
            {
                return _calculator.IsStrictlyPositive(directions.Start.Determinant(this))
                    && _calculator.IsStrictlyPositive(Determinant(directions.End));
            }

            if (_calculator.IsNegative(determinant))
            {
                return
                    _calculator.IsStrictlyNegative(directions.Start.Determinant(this))
                 && _calculator.IsStrictlyNegative(Determinant(directions.End));
            }

            return false;
        }

        public bool StrictlyBelongsTo(DirectionRange<TAlgebraicNumber> directions)
        {
            if (directions is null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            if (Equals(directions.Start) || Equals(directions.End))
            {
                return false;
            }

            return !(directions.IsShortestRange() ^ BelongsToShortestRange(directions));
        }

        public bool BelongsTo(DirectionRange<TAlgebraicNumber> directions)
        {
            if (directions is null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            if (Equals(directions.Start) || Equals(directions.End))
            {
                return true;
            }

            return !(directions.IsShortestRange() ^ BelongsToShortestRange(directions));
        }

        public Direction<TAlgebraicNumber> Opposite() =>
            new Direction<TAlgebraicNumber>(_calculator, _calculator.Opposite(X), _calculator.Opposite(Y));

        public Direction<TAlgebraicNumber> Normalize()
        {
            var length =
                _calculator.SquareRoot(
                    _calculator.Add(
                        _calculator.Multiply(X, X),
                        _calculator.Multiply(Y, Y)));

            return new Direction<TAlgebraicNumber>(
                _calculator,
                _calculator.Divide(X, length),
                _calculator.Divide(Y, length));
        }

        public Direction<TAlgebraicNumber> Scale(TAlgebraicNumber scalar) =>
            new Direction<TAlgebraicNumber>(
                _calculator,
                _calculator.Multiply(X, scalar),
                _calculator.Multiply(Y, scalar));

        public Direction<TAlgebraicNumber> NormalDirection() =>
            new Direction<TAlgebraicNumber>(_calculator, _calculator.Opposite(Y), X);

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is Direction<TAlgebraicNumber> direction)
            {
                return Equals(direction);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X!.GetHashCode() * 397) ^ Y!.GetHashCode();
            }
        }

        public static bool operator ==(Direction<TAlgebraicNumber>? left, Direction<TAlgebraicNumber>? right) =>
            Equals(left, right);

        public static bool operator !=(Direction<TAlgebraicNumber>? left, Direction<TAlgebraicNumber>? right) =>
            !Equals(left, right);
    }
}
