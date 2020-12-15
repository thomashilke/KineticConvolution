using System;

namespace KineticConvolution
{
    public class Direction
    {
        public Direction(IAlgebraicNumber x, IAlgebraicNumber y)
        {
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x.IsZero() && y.IsZero())
            {
                throw new ArgumentException(
                    "Both components of the direction cannot be simultaneously zero.",
                    nameof(y));
            }

            X = x;
            Y = y;
        }

        public IAlgebraicNumber X { get; }

        public IAlgebraicNumber Y { get; }
    }
}
