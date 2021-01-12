using System.Linq;

using FluentAssertions;

using Fractions;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class ConvolutionHelperTests
    {
        public static Point<DoubleNumber> MakePoint(double x, double y) =>
            new Point<DoubleNumber>(DoubleNumber.FromDouble(x), DoubleNumber.FromDouble(y));

        public static Tracing<DoubleNumber> MakeArc(
            Fraction weight,
            double centerX,
            double centerY,
            double directionStartX,
            double directionStartY,
            double directionEndX,
            double directionEndY,
            Orientation orientation,
            double radius) =>
            Tracing<DoubleNumber>.CreateArc(
                1,
                new Point<DoubleNumber>(DoubleNumber.FromDouble(centerX),
                                        DoubleNumber.FromDouble(centerY)),
                new DirectionRange<DoubleNumber>(
                    new Direction<DoubleNumber>(
                        DoubleNumber.FromDouble(directionStartX),
                        DoubleNumber.FromDouble(directionStartY)),
                    new Direction<DoubleNumber>(
                        DoubleNumber.FromDouble(directionEndX),
                        DoubleNumber.FromDouble(directionEndY)),
                    orientation),
                DoubleNumber.FromDouble(radius));

        [Test]
        public void When_Direction_Ranges_Are_Included_Then_Intersection_Should_Be_The_Innermost_Range()
        {
            const double radius1 = 2.0;
            const double radius2 = 2.0;

            var arc1 = (Arc<DoubleNumber>)MakeArc(
                1, 1.0, 2.0, 1.0, 0.0, 0.0, 1.0,
                Orientation.CounterClockwise,
                radius1);

            var arc2 = (Arc<DoubleNumber>)MakeArc(
                1, 2.0, 1.0, 1.0, 0.5, 0.5, 1.0,
                Orientation.CounterClockwise,
                radius2);

            var convolution = ConvolutionHelper.ConvolveArcs(arc1, arc2).ToList();

            convolution.Should().HaveCount(1);

            convolution[0].Convolution.Should().BeOfType(typeof(Arc<DoubleNumber>));

            var convolutionAsArc = convolution[0].Convolution as Arc<DoubleNumber>;

            convolutionAsArc.Center.Should().BeEquivalentTo(MakePoint(3.0, 3.0));
        }
    }
}
