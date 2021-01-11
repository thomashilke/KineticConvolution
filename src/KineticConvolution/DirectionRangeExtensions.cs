using System;

namespace Hilke.KineticConvolution
{
    public static class DirectionRangeExtensions
    {
        public static bool IsShortestRange(this DirectionRange directions) =>
            directions.Orientation switch
            {
                Orientation.Clockwise =>
                    DirectionHelper.Determinant(directions.Start, directions.End)
                                   .IsStrictlyNegative(),
                Orientation.CounterClockwise =>
                    DirectionHelper.Determinant(directions.Start, directions.End)
                                   .IsStrictlyPositive(),
                _ => throw new NotSupportedException() // TODO add a message
            };

        public static DirectionRange Reverse(this DirectionRange directions) =>
            new DirectionRange(
                directions.End,
                directions.Start,
                directions.Orientation == Orientation.CounterClockwise
                    ? Orientation.Clockwise
                    : Orientation.CounterClockwise);

        public static DirectionRange Opposite(this DirectionRange directions) =>
            new DirectionRange(
                directions.Start.Opposite(),
                directions.End.Opposite(),
                directions.Orientation);
    }
}
