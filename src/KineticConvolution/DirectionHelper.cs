namespace Hilke.KineticConvolution
{
    public static class DirectionHelper
    {
        public static IAlgebraicNumber Determinant(Direction d1, Direction d2)
        {
            var a = d1.X.MultipliedBy(d2.Y);
            var b = d1.Y.MultipliedBy(d2.X);
            return a.Subtract(b);
        }
    }
}
