using System;

namespace Hilke.KineticConvolution.Double
{
    public sealed class Point : IEquatable<Point>
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }

        public double Y { get; }

        /// <inheritdoc />
        public bool Equals(Point? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return X.Equals(other.X)
                && Y.Equals(other.Y);
        }

        public Point Translate(Direction direction, double length)
        {
            var normalizedDirection = direction.Normalize();

            return new Point(
                X + normalizedDirection.X  * length,
                Y + normalizedDirection.Y * length);
        }

        public Direction DirectionTo(Point target) =>
            new Direction(
                X - target.X,
                Y - target.Y);

        public Point Sum(Point point) =>
            new Point(X + point.X, Y + point.Y);

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

            if (obj is Point point)
            {
                return Equals(point);
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

        public static bool operator ==(Point? left, Point? right) =>
            Equals(left, right);

        public static bool operator !=(Point? left, Point? right) =>
            !Equals(left, right);
    }
}
