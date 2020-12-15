namespace KineticConvolution
{
    public static class AlgebraicNumberExtensions
    {
        public static bool IsZero(this IAlgebraicNumber number) =>
            number.Sign() == 0;
    }
}
