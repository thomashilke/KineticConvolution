using System;

namespace Hilke.KineticConvolution.Double
{
    public sealed class Direction : IEquatable<Direction>
    {
        public Direction(double x, double y)
        {
            if (x == 0.0 && y == 0.0)
            {
                throw new ArgumentException(
                    "Both components of the direction cannot be simultaneously zero.",
                    nameof(y));
            }

            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }

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

            if (GetType() != other.GetType())
            {
                return false;
            }

            return DirectionHelper.Determinant(this, other) == 0.0
                && Math.Sign(X) == Math.Sign(other.X)
                && Math.Sign(Y) == Math.Sign(other.Y);
        }

        public DirectionOrder CompareTo(Direction direction1, Direction direction2)
        {
            if (direction1.Equals(direction2))
            {
                return DirectionOrder.Equal;
            }

            return direction1.BelongsTo(
                       new DirectionRange(
                           this,
                           direction2,
                           Orientation.CounterClockwise))
                       ? DirectionOrder.After
                       : DirectionOrder.Before;
        }

        public Direction FirstOf(
            Direction direction1,
            Direction direction2) =>
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

        public Direction LastOf(
            Direction direction1,
            Direction direction2) =>
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

        public bool BelongsToShortestRange(DirectionRange directions)
        {
            var determinant = DirectionHelper.Determinant(directions.Start, directions.End);

            if (determinant > 0.0)
            {
                return
                    DirectionHelper.Determinant(directions.Start, this) > 0.0
                 && DirectionHelper.Determinant(this, directions.End) > 0.0;
            }

            if (determinant < 0.0)
            {
                return
                    DirectionHelper.Determinant(directions.Start, this) < 0.0
                 && DirectionHelper.Determinant(this, directions.End) < 0.0;
            }

            return false;
        }

        public bool BelongsTo(DirectionRange directions)
        {
            if (directions == null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            return !(directions.IsShortestRange() ^ BelongsToShortestRange(directions));
        }

        public Direction Opposite() => new Direction(-X, -Y);

        public Direction Normalize()
        {
            var length = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

            return new Direction(
                X / length,
                Y / length);
        }

        public Direction Scale(double scalar) =>
            new Direction(
                X * scalar,
                Y * scalar);

        public Direction NormalDirection() => new Direction(-Y, X);

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is Direction direction)
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

        public static bool operator ==(Direction? left, Direction? right) => Equals(left, right);

        public static bool operator !=(Direction? left, Direction? right) => !Equals(left, right);
    }
}
