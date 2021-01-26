using System;

namespace Hilke.KineticConvolution
{
    public sealed class DoubleAlgebraicNumberCalculator : IAlgebraicNumberCalculator<double>
    {
        /// <inheritdoc />
        public double Add(double left, double right) => left + right;

        /// <inheritdoc />
        public double Subtract(double left, double right) => left - right;

        /// <inheritdoc />
        public double Multiply(double left, double right) => left * right;

        /// <inheritdoc />
        public double Divide(double dividend, double divisor) => dividend / divisor;

        /// <inheritdoc />
        public double Inverse(double number)
        {
            if (number != 0.0)
            {
                return 1.0 / number;
            }

            throw new InvalidOperationException("The inverse of 0.0 is undefined.");
        }

        /// <inheritdoc />
        public double Opposite(double number) => -number;

        /// <inheritdoc />
        public int Sign(double number) => Math.Sign(number);

        /// <inheritdoc />
        public double SquareRoot(double number)
        {
            if (number >= 0.0)
            {
                return Math.Sqrt(number);
            }

            throw new InvalidOperationException("The square root of a negative number is undefined.");
        }
    }
}
