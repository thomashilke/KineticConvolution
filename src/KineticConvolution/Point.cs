using System;

namespace Hilke.KineticConvolution
{
    public sealed class Point : IEquatable<Point>
    {
        public Point(IAlgebraicNumber x, IAlgebraicNumber y)
        {
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
        }

        public IAlgebraicNumber X { get; }

        public IAlgebraicNumber Y { get; }

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

        public Point Translate(Direction direction, IAlgebraicNumber length)
        {
            var normalizedDirection = direction.Normalized();

            return new Point(
                X.Add(normalizedDirection.X.MultipliedBy(length)),
                Y.Add(normalizedDirection.Y.MultipliedBy(length)));
        }

        public Direction DirectionTo(Point target) =>
            new Direction(
                X.Subtract(target.X),
                Y.Subtract(target.Y));

        public Point Sum(Point point2) =>
            new Point(X.Add(point2.X), Y.Add(point2.Y));

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

        public static bool operator ==(Point? left, Point? right) => Equals(left, right);

        public static bool operator !=(Point? left, Point? right) => !Equals(left, right);
    }
}
