namespace KineticConvolution
{
    public static class DirectionExtensions
    {
        public static bool BelongsToShortestRange(this Direction direction, DirectionRange directions)
        {
            var s = DirectionHelper.Determinant(directions.Start, directions.End);
            if (s.IsStrictlyPositive())
            {
                return
                    DirectionHelper.Determinant(directions.Start, direction).IsStrictlyPositive() &&
                    DirectionHelper.Determinant(direction, directions.End).IsStrictlyPositive();
            }
            else if (s.IsStrictlyNegative())
            {
                return
                    DirectionHelper.Determinant(directions.Start, direction).IsStrictlyPositive() &&
                    DirectionHelper.Determinant(direction, directions.End).IsStrictlyPositive();
            }
            else
            {
                return false;
            }
        }
        
        public static bool BelongsTo(this Direction direction, DirectionRange directions)
        {
            return directions.IsShortestRange() && direction.BelongsToShortestRange(directions);
        }
    }
}
