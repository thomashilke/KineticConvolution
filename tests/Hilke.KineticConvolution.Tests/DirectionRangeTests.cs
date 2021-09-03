using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;
using Hilke.KineticConvolution.Tests.TestCaseDataSource;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionRangeTests
    {
        private IAlgebraicNumberCalculator<double> _calculator;

        [SetUp]
        public void SetUp() => _calculator = new DoubleAlgebraicNumberCalculator(zeroTolerance: 1e-10);

        [Test]
        public void When_calling_Intersection_with_null_range_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            var start = new Direction<double>(_calculator, x: 1.0, y: 0.0);
            var end = new Direction<double>(_calculator, x: 0.0, y: 1.0);

            // Act
            var subject = new DirectionRange<double>(_calculator, start, end, Orientation.CounterClockwise);

            Action action = () => _ = subject.Intersection(null!);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be(expected: "other");
        }

        [Test]
        public void When_calling_Union_with_null_range_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            var start = new Direction<double>(_calculator, x: 1.0, y: 0.0);
            var end = new Direction<double>(_calculator, x: 1.0, y: 0.0);

            var subject = new DirectionRange<double>(_calculator, start, end, Orientation.CounterClockwise);

            // Act
            Action action = () => _ = subject.Union(null);

            // Assert
            action.Should()
                .ThrowExactly<ArgumentNullException>()
                .And.ParamName.Should()
                .Be(expected: "other");
        }

        [TestCaseSource(
            typeof(DirectionRangeExtensionsIntersectionTestCaseDataSource),
            nameof(DirectionRangeExtensionsIntersectionTestCaseDataSource.TestCases))]
        public void When_calling_Intersection_with_valid_ranges_Then_the_correct_result_Should_be_returned(
            DirectionRange<double> range1,
            DirectionRange<double> range2,
            IReadOnlyList<DirectionRange<double>> expected)
        {
            // Act
            var actual1 = range1.Intersection(range2).ToList();
            var actual2 = range2.Intersection(range1).ToList();

            // Assert
            actual1.Should().HaveCount(expected.Count);
            actual1.Should().BeEquivalentTo(expected);

            actual2.Should().HaveCount(expected.Count);
            if (range1.Orientation == Orientation.CounterClockwise &&
                range2.Orientation == Orientation.CounterClockwise)
            {
                actual2.Should().BeEquivalentTo(expected);
            }
        }
    }
}
