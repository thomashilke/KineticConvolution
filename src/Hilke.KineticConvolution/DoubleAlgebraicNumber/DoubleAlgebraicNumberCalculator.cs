using System;

namespace Hilke.KineticConvolution.DoubleAlgebraicNumber
{
    public sealed class DoubleAlgebraicNumberCalculator : IAlgebraicNumberCalculator<double>
    {
        private readonly double _zeroTolerance;

        public DoubleAlgebraicNumberCalculator(double zeroTolerance = 1.0e-9)
        {
            if (!(zeroTolerance >= 0.0))
            {
                throw new ArgumentException(
                    $"The zero tolerance must be positive, but got '{zeroTolerance}'.",
                    nameof(zeroTolerance));
            }

            _zeroTolerance = zeroTolerance;
        }

        /// <inheritdoc />
        public double Add(double left, double right) => left + right;

        /// <inheritdoc />
        public double Subtract(double left, double right) => left - right;

        /// <inheritdoc />
        public double Multiply(double left, double right) => left * right;

        /// <inheritdoc />
        public double Divide(double dividend, double divisor) => dividend / divisor;

        /// <inheritdoc />
        public double Inverse(double number) => 1.0 / number;

        /// <inheritdoc />
        public double Opposite(double number) => -number;

        /// <inheritdoc />
        public int Sign(double number) => Math.Abs(number) < _zeroTolerance ? 0 : Math.Sign(number);

        /// <inheritdoc />
        public double SquareRoot(double number) => Math.Sqrt(number);

        /// <inheritdoc />
        public double CreateConstant(int value) => value;

        /// <inheritdoc />
        public double CreateConstant(double value) => value;

        /// <inheritdoc />
        public double ToDouble(double value) => value;
    }
}
