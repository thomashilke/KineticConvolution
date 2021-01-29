using System;

namespace Hilke.KineticConvolution.DoubleAlgebraicNumber
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
        public double Inverse(double number) => 1.0 / number;

        /// <inheritdoc />
        public double Opposite(double number) => -number;

        /// <inheritdoc />
        public int Sign(double number) => Math.Sign(number);

        /// <inheritdoc />
        public double SquareRoot(double number) => Math.Sqrt(number);
        
        /// <inheritdoc />
        public double CreateConstant(int value) => value;

        /// <inheritdoc />
        public double CreateConstant(double value) => value;
    }
}
