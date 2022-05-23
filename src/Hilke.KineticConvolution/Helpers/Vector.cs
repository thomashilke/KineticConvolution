using System;

namespace Hilke.KineticConvolution.Helpers
{
    public class Vector<TAlgebraicNumber>
    {
        internal Vector(TAlgebraicNumber x, TAlgebraicNumber y)
        {
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
        }

        public TAlgebraicNumber X { get; }

        public TAlgebraicNumber Y { get; }
    }
}
