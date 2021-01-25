using System;
using System.Collections.Generic;

namespace Hilke.KineticConvolution
{
    public class Convolution<TAlgebraicNumber>
        where TAlgebraicNumber : IEquatable<TAlgebraicNumber>
    {
        internal Convolution(
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
    }
}
