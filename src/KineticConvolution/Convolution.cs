using System;
using System.Collections.Generic;
using System.Linq;

using static KineticConvolution.ConvolutionHelper;

namespace KineticConvolution
{
    public class ConvolvedTracing
    {
        public ConvolvedTracing(Tracing convolution, Tracing parent1, Tracing parent2)
        {
            Convolution = convolution ?? throw new ArgumentNullException(nameof(convolution));

            Parent1 = parent1 ?? throw new ArgumentNullException(nameof(parent1));

            Parent2 = parent2 ?? throw new ArgumentNullException(nameof(parent2));
        }

        public Tracing Parent1 { get; }

        public Tracing Parent2 { get; }

        public Tracing Convolution { get; }
    }

    public class Convolution
    {
        public Convolution(IEnumerable<Tracing> shape1, IEnumerable<Tracing> shape2)
        {
            Shape1 = shape1 ?? throw new ArgumentNullException(nameof(shape1));

            Shape2 = shape2 ?? throw new ArgumentNullException(nameof(shape2));

            var convolutions =
                from tracing1 in shape1
                from tracing2 in shape2
                select Convolve(tracing1, tracing2);

            ConvolvedTracings = convolutions.SelectMany(x => x);
        }

        public IEnumerable<Tracing> Shape1 { get; }

        public IEnumerable<Tracing> Shape2 { get; }

        public IEnumerable<ConvolvedTracing> ConvolvedTracings { get; }
    }
}
