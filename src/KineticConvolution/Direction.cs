using System;

namespace Hilke.KineticConvolution
{
    public class Direction : IEquatable<Direction>
    {
        public Direction(IAlgebraicNumber x, IAlgebraicNumber y)
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

        public IAlgebraicNumber X { get; }

        public IAlgebraicNumber Y { get; }

        /// <inheritdoc />
        public bool Equals(Direction? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return DirectionHelper.Determinant(this, other).IsZero()
                && X.Sign() == other.X.Sign()
                && Y.Sign() == other.Y.Sign();
        }

        public int Compare(Direction direction1, Direction direction2)
        {
            if (direction1.Equals(direction2))
            {
                return 0;
            }

            return direction1.BelongsTo(
                       new DirectionRange(
                           this,
                           direction2,
                           Orientation.CounterClockwise))
                       ? 1
                       : -1;
        }

        public Direction FirstOfCounterClockwise(
            Direction direction1,
            Direction direction2) =>
            Compare(direction1, direction2) switch
            {
                -1 => direction2,
                1 => direction1,
                0 => direction1,
                _ => throw new NotSupportedException() // TODO add a message
            };

        public Direction LastOfCounterClockwise(
            Direction direction1,
            Direction direction2) =>
            Compare(direction1, direction2) switch
            {
                -1 => direction1,
                1 => direction2,
                0 => direction1,
                _ => throw new NotSupportedException() // TODO add a message
            };

        public bool BelongsToShortestRange(DirectionRange directions)
        {
            var s = DirectionHelper.Determinant(directions.Start, directions.End);
            if (s.IsStrictlyPositive())
            {
                return
                    DirectionHelper.Determinant(directions.Start, this).IsStrictlyPositive()
                 && DirectionHelper.Determinant(this, directions.End).IsStrictlyPositive();
            }

            if (s.IsStrictlyNegative())
            {
                return
                    DirectionHelper.Determinant(directions.Start, this).IsStrictlyPositive()
                 && DirectionHelper.Determinant(this, directions.End).IsStrictlyPositive();
            }

            return false;
        }

        public bool BelongsTo(DirectionRange directions) =>
            !(directions.IsShortestRange() ^ BelongsToShortestRange(directions));

        public Direction Opposite() => new Direction(X.Opposite(), Y.Opposite());

        public Direction Normalized()
        {
            var length = X.MultipliedBy(X)
                          .Add(Y.MultipliedBy(Y))
                          .SquareRoot();

            return new Direction(
                X.DividedBy(length),
                Y.DividedBy(length));
        }

        public Direction Scale(IAlgebraicNumber scalar) =>
            new Direction(
                X.MultipliedBy(scalar),
                Y.MultipliedBy(scalar));

        public Direction NormalDirection() => new Direction(Y.Opposite(), X);

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Direction)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Direction? left, Direction? right) => Equals(left, right);

        public static bool operator !=(Direction? left, Direction? right) => !Equals(left, right);
    }
}
