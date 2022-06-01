using System.Linq;
using System.Collections.Generic;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;
using Hilke.KineticConvolution.Extensions;
using Hilke.KineticConvolution.Tests.TestCaseDataSource;

using NUnit.Framework;
using System;

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

        [Test]
        public void When_Direction_Range_Is_Exactly_A_Half_Plan_Then_The_Shortest_Range_Should_Be_Clockwise()
        {
            var factory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

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
            typeof(DirectionRangeExtensionsTestCaseDataSource),
            nameof(DirectionRangeExtensionsTestCaseDataSource.TestCases))]
        public void When_Start_Direction_Coincide_With_End_Then_Expected_Range_Should_Be_Returned(
            Orientation orientation,
            IEnumerable<DirectionRange<double>> expectedRange)
        {
            var factory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

            var d1 = factory.CreateDirection(0.0, 1.0);
            var d2 = factory.CreateDirection(0.0, -1.0);

            var leftHalfPlan = factory.CreateDirectionRange(d1, d2, Orientation.Clockwise);
            var rightHalfPlan = factory.CreateDirectionRange(d1, d2, orientation);

            var intersection = leftHalfPlan.Intersection(rightHalfPlan);

            intersection.Should().BeEquivalentTo(expectedRange);
        }

        [TestCaseSource(
            typeof(DirectionRangeExtensionsUnionTestCaseDataSource),
            nameof(DirectionRangeExtensionsUnionTestCaseDataSource.TestCases))]
        public void When_calling_Union_with_valid_ranges_Then_the_correct_result_Should_be_returned(
            DirectionRange<double> range1,
            DirectionRange<double> range2,
            IReadOnlyList<DirectionRange<double>> expected)
        {
            // Arrange
            var expectedReversed = expected.Select(t => t.Reverse());

            // Act
            var actual1 = range1.Union(range2).ToList();
            var actual2 = range2.Union(range1).ToList();

            // Assert
            actual1.Should().HaveCount(expected.Count);
            actual1.Should().BeEquivalentTo(expected);

            actual2.Should().HaveCount(expected.Count);

            // degenerate range
            if (expected.Count == 1 && expected[0].Start == expected[0].End)
            {
                actual2[0].Start.Should().BeEquivalentTo(actual2[0].End);
                actual2[0].Orientation.Should().BeEquivalentTo(range2.Orientation);
            }
            else
            {
                if (range1.Orientation == range2.Orientation)
                {
                    actual2.Should().BeEquivalentTo(expected);
                }
                else
                {
                    actual2.Should().BeEquivalentTo(expectedReversed);
                }
            }
        }

        [TestCaseSource(
            typeof(DirectionRangeExtensionsUnionTestCaseDataSource),
            nameof(DirectionRangeExtensionsUnionTestCaseDataSource.TestCasesMultipleUnion))]
        public void When_calling_Union_on_valid_multiple_ranges_Then_the_correct_result_Should_be_returned(
            IEnumerable<DirectionRange<double>> ranges,
            IReadOnlyList<DirectionRange<double>> expected)
        {
            // Act
            var actual = ranges.Union().ToList();

            // Assert
            actual.Should().HaveCount(expected.Count);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCaseSource(
            typeof(DirectionRangeExtensionsUnionTestCaseDataSource),
            nameof(DirectionRangeExtensionsUnionTestCaseDataSource.TestCasesSortByStart))]
        public void When_calling_SortByStart_On_multiple_ranges_Should_sort_correctly(
            IEnumerable<DirectionRange<double>> unsortedRanges,
            IReadOnlyList<DirectionRange<double>> expected)
        {
            // Act
            var actual = unsortedRanges.SortCounterClockwise();

            // Assert
            actual.Should().HaveCount(expected.Count);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void When_calling_Union_with_null_source_Then_ArgumentNullException_should_be_thrown()
        {
            // Act
            Action action = () => ((IEnumerable<DirectionRange<double>>)null).Union();

            // Assert
            action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("ranges");
        }

        [Test]
        public void When_calling_Union_with_empty_source_Then_empty_collection_should_be_returned()
        {
            // Act
            var union = Enumerable.Empty<DirectionRange<double>>().Union();

            // Assert
            union.Should().BeEmpty();
        }

        [Test]
        public void When_calling_Union_with_a_single_element_then_it_should_be_returned()
        {
            // Arrange
            var factory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

            var d1 = factory.CreateDirection(1.0, 0.0);
            var d2 = factory.CreateDirection(0.0, 1.0);
            var range = factory.CreateDirectionRange(d1, d2, Orientation.CounterClockwise);
            var ranges = new[] { range };

            // Act
            var union = ranges.Union();

            // Assert
            union.Single().Should().Be(range);
        }
    }
}
