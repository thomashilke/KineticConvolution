using System;

namespace Hilke.KineticConvolution.Double
{
    public class ConvolvedTracing
    {
        public ConvolvedTracing(
            Tracing convolution,
            Tracing parent1,
            Tracing parent2)
        {
            Convolution = convolution ?? throw new ArgumentNullException(nameof(convolution));
            Parent1 = parent1 ?? throw new ArgumentNullException(nameof(parent1));
            Parent2 = parent2 ?? throw new ArgumentNullException(nameof(parent2));
        }

        public Tracing Parent1 { get; }

        public Tracing Parent2 { get; }

        public Tracing Convolution { get; }
    }
}
