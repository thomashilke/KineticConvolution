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

        public static DirectionRange Reverse(this DirectionRange directions)
        {
            return new DirectionRange(
                directions.End,
                directions.Start,
                directions.Orientation == Orientation.CounterClockwise
                ? Orientation.Clockwise
                : Orientation.CounterClockwise);
        }

        public static DirectionRange Opposite(this DirectionRange directions)
        {
            return new DirectionRange(
                directions.Start.Opposite(),
                directions.End.Opposite(),
                directions.Orientation);
        }
    }
}
