using System.Linq;

using FluentAssertions;

using Fractions;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class ConvolutionHelperTests
    {
        public static Point MakePoint(double x, double y)
        {
            return new Point(DoubleNumber.FromDouble(x), DoubleNumber.FromDouble(y));
        }

        public static Tracing MakeArc(
            Fraction weight,
            double centerX,
            double centerY,
            double directionStartX,
            double directionStartY,
            double directionEndX,
            double directionEndY,
            Orientation orientation,
            double radius) =>
            Tracing.CreateArc(
                1,
                new Point(DoubleNumber.FromDouble(centerX),
                          DoubleNumber.FromDouble(centerY)),
                new DirectionRange(
                    new Direction(
                        DoubleNumber.FromDouble(directionStartX),
                        DoubleNumber.FromDouble(directionStartY)),
                    new Direction(
                        DoubleNumber.FromDouble(directionEndX),
                        DoubleNumber.FromDouble(directionEndY)),
                    orientation),
                DoubleNumber.FromDouble(radius));

        [Test]
        public void When_Direction_Ranges_Are_Included_Then_Intersection_Should_Be_The_Innermost_Range()
        {
            const double radius1 = 2.0;
            const double radius2 = 2.0;

            var arc1 = (Arc)MakeArc(
                1, 1.0, 2.0, 1.0, 0.0, 0.0, 1.0,
                Orientation.CounterClockwise,
                radius1);

            var arc2 = (Arc)MakeArc(
                1, 2.0, 1.0, 1.0, 0.5, 0.5, 1.0,
                Orientation.CounterClockwise,
                radius2);

            var convolution = ConvolutionHelper.ConvolveArcs(arc1, arc2).ToList();

            convolution.Should().HaveCount(1);

            convolution[0].Convolution.Should().BeOfType(typeof(Arc));

            var convolutionAsArc = (convolution[0].Convolution as Arc);

            convolutionAsArc.Center.Should().BeEquivalentTo(MakePoint(3.0, 3.0));
        }
    }
}
