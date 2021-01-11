using System;
using System.Collections.Generic;
using System.Linq;

using static Hilke.KineticConvolution.ConvolutionHelper;

namespace Hilke.KineticConvolution
{
    public class Convolution
    {
        private Convolution(
            Shape shape1,
            Shape shape2,
            IReadOnlyList<ConvolvedTracing> convolvedTracings)
        {
            Shape1 = shape1 ?? throw new ArgumentNullException(nameof(shape1));

            Shape2 = shape2 ?? throw new ArgumentNullException(nameof(shape2));

            ConvolvedTracings = convolvedTracings ?? throw new ArgumentNullException(nameof(convolvedTracings));
        }

        public Shape Shape1 { get; }

        public Shape Shape2 { get; }

        public IReadOnlyList<ConvolvedTracing> ConvolvedTracings { get; }

        public static Convolution FromShapes(Shape shape1, Shape shape2)
        {
            var convolutions =
                from tracing1 in shape1.Tracings
                from tracing2 in shape2.Tracings
                select Convolve(tracing1, tracing2);

            return new Convolution(shape1, shape2, convolutions.SelectMany(x => x).ToList());
        }
    }
}
