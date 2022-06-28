#nullable disable

using Fractions;

using Hilke.KineticConvolution;
using Hilke.KineticConvolution.DoubleAlgebraicNumber;

var calculator = new DoubleAlgebraicNumberCalculator();
var factory = new ConvolutionFactory<double>(calculator);

var segment = factory.CreateSegment(
    startX: 0.0, startY: 0.0,
    endX: 1.0, endY: 2.0,
    weight: new Fraction(2, 3));

Shape<double> shape1 = null;
Shape<double> shape2 = null;

var convolution = factory.ConvolveShapes(shape1, shape2);

foreach (var convolvedTracing in convolution.ConvolvedTracings)
{
    var parent1 = convolvedTracing.Parent1;
    var parent2 = convolvedTracing.Parent2;
    var tracing = convolvedTracing.Convolution;
}
