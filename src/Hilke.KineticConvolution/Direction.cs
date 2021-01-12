using System;

namespace Hilke.KineticConvolution
{
    public sealed class Direction<TAlgebraicNumber> : IEquatable<Direction<TAlgebraicNumber>>
        where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        public Direction(TAlgebraicNumber x, TAlgebraicNumber y)
        {
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x.IsZero() && y.IsZero())
            {
                throw new ArgumentException(
                    "Both components of the direction cannot be simultaneously zero.",
                    nameof(y));
            }

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

            return DirectionHelper.Determinant(this, other).IsZero()
                && X.Sign() == other.X.Sign()
                && Y.Sign() == other.Y.Sign();
        }

        public int CompareTo(Direction<TAlgebraicNumber> direction1, Direction<TAlgebraicNumber> direction2)
        {
            if (direction1.Equals(direction2))
            {
                return 0;
            }

            return direction1.BelongsTo(
                       new DirectionRange<TAlgebraicNumber>(
                           this,
                           direction2,
                           Orientation.CounterClockwise))
                       ? 1
                       : -1;
        }

        public Direction<TAlgebraicNumber> FirstOf(
            Direction<TAlgebraicNumber> direction1,
            Direction<TAlgebraicNumber> direction2) =>
            CompareTo(direction1, direction2) switch
            {
                -1 => direction2,
                1 => direction1,
                0 => direction1,
                var sign =>
                    throw new NotSupportedException(
                        $"Comparison between two directions should yield either -1, 0 or 1, but got {sign}.")
            };

        public Direction<TAlgebraicNumber> LastOf(
            Direction<TAlgebraicNumber> direction1,
            Direction<TAlgebraicNumber> direction2) =>
            CompareTo(direction1, direction2) switch
            {
                -1 => direction1,
                1 => direction2,
                0 => direction1,
                var sign =>
                    throw new NotSupportedException(
                        $"Comparison between two directions should yield either -1, 0 or 1, but got {sign}.")
            };

        public bool BelongsToShortestRange(DirectionRange<TAlgebraicNumber> directions)
        {
            var determinant = DirectionHelper.Determinant(directions.Start, directions.End);

            if (determinant.IsStrictlyPositive())
            {
                return
                    DirectionHelper.Determinant(directions.Start, this).IsStrictlyPositive()
                 && DirectionHelper.Determinant(this, directions.End).IsStrictlyPositive();
            }

            if (determinant.IsStrictlyNegative())
            {
                return
                    DirectionHelper.Determinant(directions.Start, this).IsStrictlyNegative()
                 && DirectionHelper.Determinant(this, directions.End).IsStrictlyNegative();
            }

            return false;
        }

        public bool BelongsTo(DirectionRange<TAlgebraicNumber> directions)
        {
            if (directions == null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            return !(directions.IsShortestRange() ^ BelongsToShortestRange(directions));
        }

        public Direction<TAlgebraicNumber> Opposite() => new Direction<TAlgebraicNumber>(X.Opposite(), Y.Opposite());

        public Direction<TAlgebraicNumber> Normalize()
        {
            var length = X.Multiply(X)
                          .Add(Y.Multiply(Y))
                          .SquareRoot();

            return new Direction<TAlgebraicNumber>(
                X.Divide(length),
                Y.Divide(length));
        }

        public Direction<TAlgebraicNumber> Scale(TAlgebraicNumber scalar)
            {
            if (scalar == null)
            {
                throw new ArgumentNullException(nameof(scalar));
            }

            return new Direction<TAlgebraicNumber>(
                X.Multiply(scalar),
                Y.Multiply(scalar));
        }

        public Direction<TAlgebraicNumber> NormalDirection() => new Direction<TAlgebraicNumber>(Y.Opposite(), X);

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
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Direction<TAlgebraicNumber>? left, Direction<TAlgebraicNumber>? right) =>
            Equals(left, right);

        public static bool operator !=(Direction<TAlgebraicNumber>? left, Direction<TAlgebraicNumber>? right) =>
            !Equals(left, right);
    }
}
