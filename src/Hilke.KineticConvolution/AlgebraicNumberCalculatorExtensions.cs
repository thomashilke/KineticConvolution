using System;

namespace Hilke.KineticConvolution
{
    public static class AlgebraicNumberCalculatorExtensions
    {
        public static bool IsZero<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.Sign(number) == 0;
        }

        public static bool IsStrictlyPositive<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.Sign(number) == 1;
        }

        public static bool IsPositive<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.Sign(number) >= 0;
        }

        public static bool IsStrictlyNegative<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.Sign(number) == -1;
        }

        public static bool IsNegative<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.Sign(number) <= 0;
        }

        public static bool IsSmallerThan<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number1,
            TAlgebraicNumber number2)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsPositive(calculator.Subtract(number2, number1));
        }

        public static bool IsGreaterThan<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number1,
            TAlgebraicNumber number2)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsNegative(calculator.Subtract(number2, number1));
        }

        public static bool IsStrictlySmallerThan<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number1,
            TAlgebraicNumber number2)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsStrictlyPositive(calculator.Subtract(number2, number1));
        }

        public static bool IsStrictlyGreaterThan<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number1,
            TAlgebraicNumber number2)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsStrictlyNegative(calculator.Subtract(number2, number1));
        }

        public static TAlgebraicNumber Abs<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsStrictlyNegative(number)
                ? calculator.Opposite(number)
                : number;
        }

        public static bool AreEqual<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number1,
            TAlgebraicNumber number2)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsZero(calculator.Subtract(number2, number1));
        }

        public static bool AreClose<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number1,
            TAlgebraicNumber number2,
            TAlgebraicNumber tolerance)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            var absoluteDifference = calculator.Abs(calculator.Subtract(number2, number1));
            return calculator.IsSmallerThan(absoluteDifference, tolerance);
        }

        public static TAlgebraicNumber Min<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber first,
            TAlgebraicNumber second)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsStrictlySmallerThan(first, second) ? first : second;
        }

        public static TAlgebraicNumber Max<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber first,
            TAlgebraicNumber second)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsStrictlySmallerThan(first, second) ? second : first;
        }

        public static (TAlgebraicNumber first, TAlgebraicNumber second)
            Sort<TAlgebraicNumber>(
                this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
                TAlgebraicNumber first,
                TAlgebraicNumber second)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            return calculator.IsStrictlySmallerThan(first, second) ? (first, second) : (second, first);
        }

        public static bool IsInsideRange<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number,
            TAlgebraicNumber lowerBound,
            TAlgebraicNumber upperBound,
            bool lowerBoundIncluded = true,
            bool upperBoundIncluded = true)
        {
            if (calculator is null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            var isGreaterThanLowerBound =
                lowerBoundIncluded
                    ? calculator.IsGreaterThan(number, lowerBound)
                    : calculator.IsStrictlyGreaterThan(number, lowerBound);

            var isSmallerThanUpperBound =
                upperBoundIncluded
                    ? calculator.IsSmallerThan(number, upperBound)
                    : calculator.IsStrictlySmallerThan(number, lowerBound);

            return isGreaterThanLowerBound && isSmallerThanUpperBound;
        }

        public static bool IsStrictlyInsideRange<TAlgebraicNumber>(
            this IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            TAlgebraicNumber number,
            TAlgebraicNumber lowerBound,
            TAlgebraicNumber upperBound) =>
            IsInsideRange(calculator, number, lowerBound, upperBound, false, false);
    }
}
