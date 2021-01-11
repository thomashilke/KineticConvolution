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
