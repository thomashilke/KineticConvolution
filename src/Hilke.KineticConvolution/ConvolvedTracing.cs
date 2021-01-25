using System;

namespace Hilke.KineticConvolution
{
    public class ConvolvedTracing<TAlgebraicNumber>
        where TAlgebraicNumber : IEquatable<TAlgebraicNumber>
    {
        public ConvolvedTracing(
            Tracing<TAlgebraicNumber> convolution,
            Tracing<TAlgebraicNumber> parent1,
            Tracing<TAlgebraicNumber> parent2)
        {
            Convolution = convolution ?? throw new ArgumentNullException(nameof(convolution));
            Parent1 = parent1 ?? throw new ArgumentNullException(nameof(parent1));
            Parent2 = parent2 ?? throw new ArgumentNullException(nameof(parent2));
        }

        public Tracing<TAlgebraicNumber> Parent1 { get; }

        public Tracing<TAlgebraicNumber> Parent2 { get; }

        public Tracing<TAlgebraicNumber> Convolution { get; }
    }
}
