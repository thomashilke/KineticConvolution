using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public class Arc : Tracing
    {
        public Point Center { get; }

        public DirectionRange Directions { get; }

        public IAlgebraicNumber Radius { get; }

        public Arc(Fraction weight, Point center, DirectionRange directions, IAlgebraicNumber radius)
            : base(weight)
        {
            Center = center ?? throw new ArgumentNullException(nameof(center));

            Directions = directions ?? throw new ArgumentNullException(nameof(directions));

            Radius = radius ?? throw new ArgumentNullException(nameof(radius));
        }
    }
}
