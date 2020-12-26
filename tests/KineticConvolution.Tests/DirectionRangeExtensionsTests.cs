using NUnit.Framework;

using FluentAssertions;

using KineticConvolution;

namespace KineticConvolutionTests
{
    [TestFixture]
    public class DirectionRangeExtensionsTests
    {
        [Test]
        public void IsShortestRange_Should_Return_Expected_Result()
        {
            var d1 = new Direction(DoubleNumber.FromDouble(1.0), DoubleNumber.FromDouble(0.0));
            var d2 = new Direction(DoubleNumber.FromDouble(0.0), DoubleNumber.FromDouble(1.0));

            var range1 = new DirectionRange(d1, d2, Orientation.CounterClockwise);
            var range2 = new DirectionRange(d1, d2, Orientation.Clockwise);
            var range3 = new DirectionRange(d2, d1, Orientation.CounterClockwise);
            var range4 = new DirectionRange(d2, d1, Orientation.Clockwise);

            range1.IsShortestRange().Should().BeTrue();
            range2.IsShortestRange().Should().BeFalse();
            range3.IsShortestRange().Should().BeFalse();
            range4.IsShortestRange().Should().BeTrue();
        }
    }
}
