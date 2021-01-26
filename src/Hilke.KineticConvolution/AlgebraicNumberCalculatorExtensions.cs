using System;

namespace Hilke.KineticConvolution
{
    public static class AlgebraicNumberCalculatorExtensions
    {
        public static bool IsZero<TAlgebraicNumber>(this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator, TAlgebraicNumber number)
        {
            if (calculator == null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.Sign(number) == 0;
        }

        public static bool IsStrictlyPositive<TAlgebraicNumber>(this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator, TAlgebraicNumber number)
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return calculator.Sign(number) == 1;
        }

        public static bool IsPositive<TAlgebraicNumber>(this IAlgebraicNumber<TAlgebraicNumber> number)
            where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Sign() >= 0;
        }

        public static bool IsStrictlyNegative<TAlgebraicNumber>(this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator, TAlgebraicNumber number)
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return calculator.Sign(number) == -1;
        }

        public static bool IsNegative<TAlgebraicNumber>(this IAlgebraicNumber<TAlgebraicNumber> number)
            where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Sign() <= 0;
        }
    }
}
