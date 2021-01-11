using Fractions;

namespace Hilke.KineticConvolution
{
    public abstract class Tracing
    {
        public Tracing(Fraction weight) => Weight = weight;

        public Fraction Weight { get; }
    }
}
