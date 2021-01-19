using System;

namespace Hilke.KineticConvolution.Double
{
    public static class AlgebraicNumberExtensions
    {
        public static bool IsZero<TAlgebraicNumber>(this IAlgebraicNumber<TAlgebraicNumber> number)
            where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Sign() == 0;
        }

        public static bool IsStrictlyPositive<TAlgebraicNumber>(this IAlgebraicNumber<TAlgebraicNumber> number)
            where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Sign() == 1;
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

        public static bool IsStrictlyNegative<TAlgebraicNumber>(this IAlgebraicNumber<TAlgebraicNumber> number)
            where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
        {
            if (number == null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            return number.Sign() == -1;
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
