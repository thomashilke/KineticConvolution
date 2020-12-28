using System;

namespace KineticConvolution
{
    public class Point : IEquatable<Point>
    {
        public IAlgebraicNumber X { get; }

        public IAlgebraicNumber Y { get; }

        public Point(IAlgebraicNumber x, IAlgebraicNumber y)
        {
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
        }

        public bool Equals(Point point)
        {
            return X.Equals(point.X) && Y.Equals(point.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point point)
            {
                return Equals(point);
            }

            return false;
        }
    }
}
