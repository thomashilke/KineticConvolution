namespace Hilke.KineticConvolution
{
    internal static class DirectionHelper
    {
        public static IAlgebraicNumber<TAlgebraicNumber> Determinant<TAlgebraicNumber>(
            Direction<TAlgebraicNumber> d1,
            Direction<TAlgebraicNumber> d2) where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
        {
            var a = d1.X.Multiply(d2.Y);
            var b = d1.Y.Multiply(d2.X);
            return a.Subtract(b);
        }
    }
}
