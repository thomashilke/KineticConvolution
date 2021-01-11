using System;
using System.Collections.Generic;
using System.Linq;

using static Hilke.KineticConvolution.ConvolutionHelper;

namespace Hilke.KineticConvolution
{
    public class Convolution
    {
        private Convolution(
            IReadOnlyList<Tracing> shape1,
            IReadOnlyList<Tracing> shape2,
            IReadOnlyList<ConvolvedTracing> convolvedTracings)
        {
            Shape1 = shape1 ?? throw new ArgumentNullException(nameof(shape1));

            Shape2 = shape2 ?? throw new ArgumentNullException(nameof(shape2));

            ConvolvedTracings = convolvedTracings ?? throw new ArgumentNullException(nameof(convolvedTracings));
        }

        public IReadOnlyList<Tracing> Shape1 { get; }

        public IReadOnlyList<Tracing> Shape2 { get; }

        public IReadOnlyList<ConvolvedTracing> ConvolvedTracings { get; }

        public static Convolution FromShapes(IEnumerable<Tracing> shape1, IEnumerable<Tracing> shape2)
        {
            var shape1Enumerated = shape1.ToList();
            var shape2Enumerated = shape2.ToList();

            var convolutions =
                from tracing1 in shape1Enumerated
                from tracing2 in shape2Enumerated
                select Convolve(tracing1, tracing2);

            return new Convolution(shape1Enumerated, shape2Enumerated, convolutions.SelectMany(x => x).ToList());
        }
    }
}
