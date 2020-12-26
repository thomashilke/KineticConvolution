using System;

namespace KineticConvolution
{
    public static class DirectionRangeExtensions
    {
        public static bool IsShortestRange(this DirectionRange directions)
        {
            switch (directions.Orientation)
            {
                case Orientation.Clockwise:
                    return DirectionHelper.Determinant(directions.Start, directions.End)
                        .IsStrictlyNegative();

                case Orientation.CounterClockwise:
                    return DirectionHelper.Determinant(directions.Start, directions.End)
                        .IsStrictlyPositive();

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
