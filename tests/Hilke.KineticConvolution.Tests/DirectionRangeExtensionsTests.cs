using FluentAssertions;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionRangeExtensionsTests
    {
        [Test]
        public void IsShortestRange_Should_Return_Expected_Result()
        {
            var factory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

            var d1 = factory.CreateDirection(1.0, 0.0);
            var d2 = factory.CreateDirection(0.0, 1.0);

            var range1 = factory.CreateDirectionRange(d1, d2, Orientation.CounterClockwise);
            var range2 = factory.CreateDirectionRange(d1, d2, Orientation.Clockwise);
            var range3 = factory.CreateDirectionRange(d2, d1, Orientation.CounterClockwise);
            var range4 = factory.CreateDirectionRange(d2, d1, Orientation.Clockwise);

            range1.IsShortestRange().Should().BeTrue();
            range2.IsShortestRange().Should().BeFalse();
            range3.IsShortestRange().Should().BeFalse();
            range4.IsShortestRange().Should().BeTrue();
        }
    }
}
