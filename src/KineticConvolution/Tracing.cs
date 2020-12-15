using Fractions;

using System;

namespace KineticConvolution
{
    public abstract class Tracing
    {
        public Fraction Weight { get; }

        public Tracing(Fraction weight)
        {
            Weight = weight;
        }
    }
}
