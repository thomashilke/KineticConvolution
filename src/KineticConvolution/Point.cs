using System;

namespace KineticConvolution
{
    public class Point
    {
        public IAlgebraicNumber X { get; }

        public IAlgebraicNumber Y { get; }

        public Point(IAlgebraicNumber x, IAlgebraicNumber y)
        {
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
        }
    }
}
