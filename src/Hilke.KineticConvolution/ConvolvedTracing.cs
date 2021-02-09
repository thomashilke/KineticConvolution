using System;
using System.Diagnostics;

namespace Hilke.KineticConvolution
{
    [DebuggerDisplay("Convolution(Convolution: {Convolution}, Parent1: {Parent1}, Parent2: {Parent2})")]
    public sealed class ConvolvedTracing<TAlgebraicNumber>
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
