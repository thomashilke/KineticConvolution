namespace KineticConvolution
{
    public static class AlgebraicNumberExtensions
    {
        public static bool IsZero(this IAlgebraicNumber number) =>
            number.Sign() == 0;

        public static bool IsStrictlyPositive(this IAlgebraicNumber number) =>
            number.Sign() == 1;

        public static bool IsPositive(this IAlgebraicNumber number) =>
            number.Sign() >= 0;

        public static bool IsStrictlyNegative(this IAlgebraicNumber number) =>
            number.Sign() == -1;

        public static bool IsNegative(this IAlgebraicNumber number) =>
            number.Sign() <= 0;
    }
}
