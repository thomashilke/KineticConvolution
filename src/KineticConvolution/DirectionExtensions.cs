namespace Hilke.KineticConvolution
{
    public static class DirectionExtensions
    {
        public static bool BelongsToShortestRange(this Direction direction, DirectionRange directions)
        {
            var s = DirectionHelper.Determinant(directions.Start, directions.End);
            if (s.IsStrictlyPositive())
            {
                return
                    DirectionHelper.Determinant(directions.Start, direction).IsStrictlyPositive()
                 && DirectionHelper.Determinant(direction, directions.End).IsStrictlyPositive();
            }

            if (s.IsStrictlyNegative())
            {
                return
                    DirectionHelper.Determinant(directions.Start, direction).IsStrictlyPositive()
                 && DirectionHelper.Determinant(direction, directions.End).IsStrictlyPositive();
            }

            return false;
        }

        public static bool BelongsTo(this Direction direction, DirectionRange directions) =>
            !(directions.IsShortestRange() ^ direction.BelongsToShortestRange(directions));

        public static Direction Opposite(this Direction direction) =>
            new Direction(direction.X.Opposite(), direction.Y.Opposite());

        public static Direction Normalized(this Direction direction)
        {
            var length = direction.X.MultipliedBy(direction.X)
                                  .Add(direction.Y.MultipliedBy(direction.Y))
                                  .SquareRoot();

            return new Direction(
                direction.X.DividedBy(length),
                direction.Y.DividedBy(length));
        }

        public static Direction Scale(this Direction direction, IAlgebraicNumber scalar) =>
            new Direction(
                direction.X.MultipliedBy(scalar),
                direction.Y.MultipliedBy(scalar));
    }
}
