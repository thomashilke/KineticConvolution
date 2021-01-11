using System;

namespace Hilke.KineticConvolution
{
    public class Point : IEquatable<Point>
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

        public Direction DirectionTo(Point target) =>
            new Direction(
                X.Subtract(target.X),
                Y.Subtract(target.Y));

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

            return obj.GetType() == GetType() && Equals((Point)obj);
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
