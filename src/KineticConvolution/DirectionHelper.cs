namespace Hilke.KineticConvolution
{
    internal static class DirectionHelper
    {
        public static IAlgebraicNumber Determinant(Direction d1, Direction d2)
        {
            var a = d1.X.MultiplyBy(d2.Y);
            var b = d1.Y.MultiplyBy(d2.X);
            return a.Subtract(b);
        }
    }
}
