using System;

namespace Hilke.KineticConvolution.Double
{
    public sealed class Point<TAlgebraicNumber> : IEquatable<Point<TAlgebraicNumber>>
        where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        public Point(TAlgebraicNumber x, TAlgebraicNumber y)
        {
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
        }

        public TAlgebraicNumber X { get; }

        public TAlgebraicNumber Y { get; }

        /// <inheritdoc />
        public bool Equals(Point<TAlgebraicNumber>? other)
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

        public Point<TAlgebraicNumber> Translate(Direction<TAlgebraicNumber> direction, TAlgebraicNumber length)
        {
            var normalizedDirection = direction.Normalize();

            return new Point<TAlgebraicNumber>(
                X.Add(normalizedDirection.X.Multiply(length)),
                Y.Add(normalizedDirection.Y.Multiply(length)));
        }

        public Direction<TAlgebraicNumber> DirectionTo(Point<TAlgebraicNumber> target) =>
            new Direction<TAlgebraicNumber>(
                X.Subtract(target.X),
                Y.Subtract(target.Y));

        public Point<TAlgebraicNumber> Sum(Point<TAlgebraicNumber> point2) =>
            new Point<TAlgebraicNumber>(X.Add(point2.X), Y.Add(point2.Y));

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

            if (obj is Point<TAlgebraicNumber> point)
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

        public static bool operator ==(Point<TAlgebraicNumber>? left, Point<TAlgebraicNumber>? right) =>
            Equals(left, right);

        public static bool operator !=(Point<TAlgebraicNumber>? left, Point<TAlgebraicNumber>? right) =>
            !Equals(left, right);
    }
}
