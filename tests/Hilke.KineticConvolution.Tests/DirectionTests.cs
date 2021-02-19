using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionTests
    {
        private static ConvolutionFactory _factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new ConvolutionFactory();
        }

        [Test]
        public void When_Direction_Is_Given_Then_It_Should_Belongs_To_The_Expected_Half_Plan()
        {
            var east = _factory.CreateDirection(3.0, 0.0);
            var west = _factory.CreateDirection(-2.0, 0.0);

            var lowerHalfPlan = _factory.CreateDirectionRange(east, west, Orientation.Clockwise);
            var upperHalfPlan = _factory.CreateDirectionRange(east, west, Orientation.CounterClockwise);

            var directionInUpperHalfPlan = _factory.CreateDirection(-5.0, 0.5);
            var directionInLowerHalfPlan = _factory.CreateDirection(-5.0, -0.5);

            directionInUpperHalfPlan.BelongsTo(lowerHalfPlan).Should().BeFalse();
            directionInUpperHalfPlan.BelongsTo(upperHalfPlan).Should().BeTrue();

            directionInLowerHalfPlan.BelongsTo(lowerHalfPlan).Should().BeTrue();
            directionInLowerHalfPlan.BelongsTo(upperHalfPlan).Should().BeFalse();
        }

        [Test]
        public void When_Direction_Is_One_DirectionRange_Extremity_Then_Direction_Should_Not_Belongs_Strictly_To_DirectionRange()
        {
            var east = _factory.CreateDirection(1.0, 0.0);
            var north = _factory.CreateDirection(0.0, 1.0);

            var directionRange1 = _factory.CreateDirectionRange(east, north, Orientation.CounterClockwise);
            var directionRange2 = _factory.CreateDirectionRange(east, north, Orientation.Clockwise);

            east.BelongsTo(directionRange1).Should().BeTrue();
            east.BelongsTo(directionRange2).Should().BeTrue();
            north.BelongsTo(directionRange1).Should().BeTrue();
            north.BelongsTo(directionRange2).Should().BeTrue();

            east.StrictlyBelongsTo(directionRange1).Should().BeFalse();
            east.StrictlyBelongsTo(directionRange2).Should().BeFalse();
            north.StrictlyBelongsTo(directionRange1).Should().BeFalse();
            north.StrictlyBelongsTo(directionRange2).Should().BeFalse();
        }

        [Test]
        public void When_DirectionRange_Is_The_Complete_Circle_Then_All_Directions_Should_Belong_To_It()
        {
            // Arrange
            var east = _factory.CreateDirection(5.0, 0.0);
            var west = _factory.CreateDirection(-5.0, 0.0);

            var clockwiseRange = _factory.CreateDirectionRange
                (east, east, Orientation.Clockwise);

            var counterClockwiseRange = _factory.CreateDirectionRange(
                east, east, Orientation.CounterClockwise);

            // Act
            var westBelongsToClockwiseRange = west.BelongsTo(clockwiseRange);
            var westBelongsToCounterClockwiseRange = west.BelongsTo(counterClockwiseRange);
            var eastBelongsToClockwiseRange = east.BelongsTo(clockwiseRange);
            var eastBelongsToCounterClockwiseRange = east.BelongsTo(counterClockwiseRange);

            // Assert
            westBelongsToClockwiseRange.Should().BeTrue();
            westBelongsToCounterClockwiseRange.Should().BeTrue();
            westBelongsToClockwiseRange.Should().BeTrue();
            westBelongsToCounterClockwiseRange.Should().BeTrue();
        }

        private static IEnumerable<TestCaseData> DirectionEqualityTestCaseSource()
        {
            var factory = new ConvolutionFactory();

            yield return new TestCaseData(
                factory.CreateDirection(1.0, 2.0),
                factory.CreateDirection(2.0, 4.0),
                true).SetName("When_Directions_Only_Change_In_Length_Then_Directions_Should_Be_Equal");

            var equalityTolerance = 1.0e-9;

            var north = factory.CreateDirection(0.0, 1.0);
            var east = factory.CreateDirection(5.0, 0.0);
            var south = factory.CreateDirection(0.0, -1.0);
            var west = factory.CreateDirection(-5.0, 0.0);

            var cardinalDirections = new [] {north, east, south, west};

            foreach (var cardinalDirection in cardinalDirections)
            {
                yield return new TestCaseData(cardinalDirection, cardinalDirection, true)
                    .SetName("When_Direction_Is_A_Cardinal_Direction_Then_It_Should_Equals_Itself");
            }

            foreach (var directions in cardinalDirections
                .SelectMany(direction =>
                    perturb(direction, 0.1 * equalityTolerance)
                    .Select(perturbedDirection => (direction, perturbedDirection))))
            {
                yield return new TestCaseData(directions.direction, directions.perturbedDirection, true)
                    .SetName("When_Directions_Are_Perturbed_Within_Tolerance_Then_They_Should_Equal");
            }

            foreach (var directions in cardinalDirections
                .SelectMany(direction =>
                    perturb(direction, 10000.0 * equalityTolerance)
                    .Select(perturbedDirection => (direction, perturbedDirection))))
            {
                yield return new TestCaseData(directions.direction, directions.perturbedDirection, false)
                    .SetName("When_Directions_Are_Perturbed_Beyond_Tolerance_Then_They_Should_Not_Equal");
            }

            yield return new TestCaseData(north, south, false)
                .SetName("When_Directions_Are_Opposite_Then_Directions_Should_Not_Equal");

            yield return new TestCaseData(west, east, false)
                .SetName("When_Directions_Are_Opposite_Then_Directions_Should_Not_Equal");

            IEnumerable<Direction<double>> perturb(Direction<double> direction, double tolerance)
            {
                yield return factory.CreateDirection(direction.X + tolerance, direction.Y + tolerance);
                yield return factory.CreateDirection(direction.X - tolerance, direction.Y - tolerance);
                yield return factory.CreateDirection(direction.X + tolerance, direction.Y - tolerance);
                yield return factory.CreateDirection(direction.X - tolerance, direction.Y + tolerance);
            }
        }

        [TestCaseSource(nameof(DirectionEqualityTestCaseSource))]
        public void When_test_equality_of_directions(
            Direction<double> direction1,
            Direction<double> direction2,
            bool expectedAreEquals) =>
        direction1.Equals(direction2).Should().Be(expectedAreEquals);
    }
}
