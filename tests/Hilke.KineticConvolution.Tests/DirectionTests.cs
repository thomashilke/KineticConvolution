using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionTests
    {
        private ConvolutionFactory _factory;

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
    }
}
