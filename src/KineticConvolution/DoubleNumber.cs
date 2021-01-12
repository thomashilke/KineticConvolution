using System;

namespace Hilke.KineticConvolution
{
    public sealed class DoubleNumber : IAlgebraicNumber, IEquatable<DoubleNumber>
    {
        private DoubleNumber(double value) => Value = value;

        public double Value { get; }

        public IAlgebraicNumber Add(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(n.Value + Value);
            }

            throw new NotSupportedException();
        }

        public IAlgebraicNumber Subtract(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(Value - n.Value);
            }

            throw new NotSupportedException();
        }

        public IAlgebraicNumber MultipliedBy(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(n.Value * Value);
            }

            throw new NotSupportedException();
        }

        public IAlgebraicNumber DividedBy(IAlgebraicNumber operand)
        {
            if (operand is DoubleNumber n)
            {
                return new DoubleNumber(Value / n.Value);
            }

            throw new NotSupportedException();
        }

        public IAlgebraicNumber Inverse()
        {
            if (Value != 0.0)
            {
                return new DoubleNumber(1.0 / Value);
            }

            throw new InvalidOperationException($"The inverse of 0.0 is undefined.");
        }

        public IAlgebraicNumber SquareRoot()
        {
            if (Value >= 0.0)
            {
                return new DoubleNumber(Math.Sqrt(Value));
            }

            throw new InvalidOperationException("The square root of a negative number is undefined.");
        }

        public IAlgebraicNumber Opposite() => new DoubleNumber(-Value);

        public int Sign() => Math.Sign(Value);

        /// <inheritdoc />
        public bool Equals(DoubleNumber? other)
        {
            if (other is null)
            {
                return false;
            }

            return ReferenceEquals(this, other) || Value.Equals(other.Value);
        }

        public bool Equals(IAlgebraicNumber? other) => Equals(other as DoubleNumber);

        public static DoubleNumber FromDouble(double value) => new DoubleNumber(value);

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            ReferenceEquals(this, obj) || obj is DoubleNumber other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(DoubleNumber? left, DoubleNumber? right) => Equals(left, right);

        public static bool operator !=(DoubleNumber? left, DoubleNumber? right) => !Equals(left, right);
    }
}
