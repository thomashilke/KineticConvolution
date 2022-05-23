using System;
using System.Collections.Generic;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;
using Hilke.KineticConvolution.Helpers;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.Helpers
{
    [TestFixture(TestOf = typeof(GeometryCalculator<>))]
    internal class GeometryCalculatorTests
    {

        private static readonly DoubleAlgebraicNumberCalculator DoubleCalculator = new();
        private static readonly ConvolutionFactory<double> Factory = new(DoubleCalculator);

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = new GeometryCalculator<double>(DoubleCalculator);

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = new GeometryCalculator<double>(DoubleCalculator);

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<GeometryCalculator<double>>();
        }

        [Test]
        public void Given_invalid_parameters_When_calling_constructor_Then_throw_exception()
        {
            // Arrange
            Action action = () => _ = new GeometryCalculator<double>(calculator: null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("calculator");
        }

        [TestCaseSource(nameof(TestCases))]

        public void Given_a_testcase_When_CreateCornerArc_Then_returns_expected(
            Direction<double> directionBefore,
            Point<double> cornerArcPoint,
            Direction<double> directionAfter,
            double radius,
            Arc<double>? expectedArc
            )
        {
            // Arrange
            var subject = new GeometryCalculator<double>(DoubleCalculator);

            // Act
            var actualArc = subject.CreateCornerArc(
                directionBefore,
                cornerArcPoint,
                directionAfter,
                radius);

            // Assert
            actualArc.Should().BeEquivalentTo(expectedArc);
        }

        public static IEnumerable<TestCaseData> TestCases()
        {
            yield return TestCase01();
            yield return TestCase02();
            yield return TestCase03();
            yield return TestCase04();
        }

        public static TestCaseData TestCase01()
        {
            var directionBefore = Factory.CreateDirection(-1.0, 0.0);
            var cornerPoint = Factory.CreatePoint(0.0, 0.0);
            var directionAfter = Factory.CreateDirection(0.0, 1.0);
            var radius = 0.1;

            var expectedArc = Factory.CreateArc(
                    center: Factory.CreatePoint(0.1, 0.1),
                    directions: Factory.CreateDirectionRange(
                        start: Factory.CreateDirection(0.0, -1.0),
                        end: Factory.CreateDirection(-1.0, 0.0),
                        orientation: Orientation.Clockwise),
                    radius: radius,
                    weight: 1);

            return new TestCaseData(directionBefore, cornerPoint, directionAfter, radius, expectedArc)
                .SetArgDisplayNames(nameof(TestCase01));
        }

        public static TestCaseData TestCase02()
        {
            var directionBefore = Factory.CreateDirection(1.0, 0.0);
            var cornerPoint = Factory.CreatePoint(1.0, 0.0);
            var directionAfter = Factory.CreateDirection(0.0, 1.0);
            var radius = 0.1;

            var expectedArc = Factory.CreateArc(
                center: Factory.CreatePoint(0.9, 0.1),
                directions: Factory.CreateDirectionRange(
                    start: Factory.CreateDirection(0.0, -1.0),
                    end: Factory.CreateDirection(1.0, 0.0),
                    orientation: Orientation.CounterClockwise),
                radius: radius,
                weight: 1);

            return new TestCaseData(directionBefore, cornerPoint, directionAfter, radius, expectedArc)
                .SetArgDisplayNames(nameof(TestCase02));
        }

        public static TestCaseData TestCase03()
        {
            var directionBefore = Factory.CreateDirection(1.0, 0.0);
            var cornerPoint = Factory.CreatePoint(1.0, 0.0);
            var directionAfter = Factory.CreateDirection(-1.0, 0.0);
            var radius = 0.0;

            var expectedArc = Factory.CreateArc(
                center: cornerPoint,
                directions: Factory.CreateDirectionRange(
                    start: Factory.CreateDirection(0.0, -1.0),
                    end: Factory.CreateDirection(0.0, 1.0),
                    orientation: Orientation.CounterClockwise),
                radius: radius,
                weight: 1);

            return new TestCaseData(directionBefore, cornerPoint, directionAfter, radius, expectedArc)
                .SetArgDisplayNames(nameof(TestCase03));
        }

        public static TestCaseData TestCase04()
        {
            var directionBefore = Factory.CreateDirection(1.0, 0.0);
            var cornerPoint = Factory.CreatePoint(1.0, 0.0);
            var directionAfter = Factory.CreateDirection(1.0, 0.0);
            var radius = 0.1;

            Arc<double>? expectedArc = null;

            return new TestCaseData(directionBefore, cornerPoint, directionAfter, radius, expectedArc)
                .SetArgDisplayNames(nameof(TestCase04));
        }
    }
}


