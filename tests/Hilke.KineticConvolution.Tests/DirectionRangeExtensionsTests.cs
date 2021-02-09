using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution.Tests
{
    internal static class DirectionRangeTestCaseDataSource
    {
        public static IEnumerable<TestCaseData> TestCases()
        {
            var factory = new ConvolutionFactory();

            yield return new TestCaseData(
                Orientation.CounterClockwise, Enumerable.Empty<DirectionRange<double>>());

            yield return new TestCaseData(
                Orientation.Clockwise,
                new []
                {
                    factory.CreateDirectionRange(
                        factory.CreateDirection(0, 1),
                        factory.CreateDirection(0, -1),
                        Orientation.Clockwise)
                });
        }
    }

    [TestFixture]
    public class DirectionRangeExtensionsTests
    {
        [Test]
        public void IsShortestRange_Should_Return_Expected_Result()
        {
            var factory = new ConvolutionFactory();

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

        [Test]
        public void When_Direction_Range_Is_Exactly_A_Half_Plan_Then_The_Shortest_Range_Should_Be_Clockwise()
        {
            var factory = new ConvolutionFactory();

            var d1 = factory.CreateDirection(1.0, 0.0);
            var d2 = factory.CreateDirection(-1.0, 0.0);

            var shortestRange1 = factory.CreateDirectionRange(d1, d2, Orientation.Clockwise);
            var shortestRange2 = factory.CreateDirectionRange(d2, d1, Orientation.Clockwise);

            var longestRange1 = factory.CreateDirectionRange(d1, d2, Orientation.CounterClockwise);
            var longestRange2 = factory.CreateDirectionRange(d2, d1, Orientation.CounterClockwise);

            shortestRange1.IsShortestRange().Should().BeTrue();
            shortestRange2.IsShortestRange().Should().BeTrue();

            longestRange1.IsShortestRange().Should().BeFalse();
            longestRange2.IsShortestRange().Should().BeFalse();
        }

        [TestCaseSource(
            typeof(DirectionRangeTestCaseDataSource),
            nameof(DirectionRangeTestCaseDataSource.TestCases))]
        public void When_Start_Direction_Coincide_With_End_Then_Expected_Range_Should_Be_Returned(
            Orientation orientation,
            IEnumerable<DirectionRange<double>> expectedRange)
        {
            var factory = new ConvolutionFactory();

            var d1 = factory.CreateDirection(0.0, 1.0);
            var d2 = factory.CreateDirection(0.0, -1.0);

            var leftHalfPlan = factory.CreateDirectionRange(d1, d2, Orientation.Clockwise);
            var rightHalfPlan = factory.CreateDirectionRange(d1, d2, orientation);

            var intersection = leftHalfPlan.Intersection(rightHalfPlan);

            intersection.Should().BeEquivalentTo(expectedRange);
        }
    }
}
