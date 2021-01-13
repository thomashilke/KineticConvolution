using System;
using System.Collections.Generic;
using System.Linq;

using static Hilke.KineticConvolution.ConvolutionHelper;

namespace Hilke.KineticConvolution
{
    public class Convolution<TAlgebraicNumber> where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        private Convolution(
            Shape<TAlgebraicNumber> shape1,
            Shape<TAlgebraicNumber> shape2,
            IReadOnlyList<ConvolvedTracing<TAlgebraicNumber>> convolvedTracings)
        {
            Shape1 = shape1 ?? throw new ArgumentNullException(nameof(shape1));
            Shape2 = shape2 ?? throw new ArgumentNullException(nameof(shape2));
            
            ConvolvedTracings = convolvedTracings ?? throw new ArgumentNullException(nameof(convolvedTracings));
        }

        public Shape<TAlgebraicNumber> Shape1 { get; }

        public Shape<TAlgebraicNumber> Shape2 { get; }

        public IReadOnlyList<ConvolvedTracing<TAlgebraicNumber>> ConvolvedTracings { get; }

        public static Convolution<TAlgebraicNumber> FromShapes(
            Shape<TAlgebraicNumber> shape1,
            Shape<TAlgebraicNumber> shape2)
        {
            var convolutions =
                from tracing1 in shape1.Tracings
                from tracing2 in shape2.Tracings
                select Convolve(tracing1, tracing2);

            return new Convolution<TAlgebraicNumber>(shape1, shape2, convolutions.SelectMany(x => x).ToList());
        }
    }
}
