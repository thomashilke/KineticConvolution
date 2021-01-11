namespace Hilke.KineticConvolution
{
    public static class PointExtensions
    {
        public static Point Translate(this Point point, Direction direction, IAlgebraicNumber length)
        {
            var normalizedDirection = direction.Normalized();

            return new Point(
                point.X.Add(normalizedDirection.X.MultipliedBy(length)),
                point.Y.Add(normalizedDirection.Y.MultipliedBy(length)));
        }
    }
}
