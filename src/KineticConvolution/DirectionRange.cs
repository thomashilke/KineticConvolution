using System;
using System.ComponentModel;

namespace Hilke.KineticConvolution
{
    public class DirectionRange
    {
        public DirectionRange(
            Direction start,
            Direction end,
            Orientation orientation)
        {
            Start = start ?? throw new ArgumentNullException(nameof(start));

            End = end ?? throw new ArgumentNullException(nameof(end));

            if (!Enum.IsDefined(typeof(Orientation), orientation))
            {
                throw new InvalidEnumArgumentException(
                    nameof(orientation),
                    (int)orientation,
                    typeof(Orientation));
            }

            Orientation = orientation;
        }

        public Direction Start { get; }

        public Direction End { get; }

        public Orientation Orientation { get; }
    }
}
