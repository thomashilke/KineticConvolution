using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionExtensionsTests
    {
        [Test]
        public void When_Direction_Is_Given_Then_BelongsTo_Should_Return_Expected_Result()
        {
            var factory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

            var east = factory.CreateDirection(1.0, 0.0);
            var north = factory.CreateDirection(0.0, 1.0);

            var clockwiseRange = factory.CreateDirectionRange(east, north, Orientation.Clockwise);
            var counterClockwiseRange = factory.CreateDirectionRange(east, north, Orientation.CounterClockwise);

            var northEast = factory.CreateDirection(1.0, 1.0);
            var southWest = northEast.Opposite();

            northEast.BelongsTo(clockwiseRange).Should().BeFalse();
            northEast.BelongsTo(counterClockwiseRange).Should().BeTrue();

            southWest.BelongsTo(clockwiseRange).Should().BeTrue();
            southWest.BelongsTo(counterClockwiseRange).Should().BeFalse();
        }

        [Test]
        public void When_Direction_Is_Close_To_Range_Boundary_Then_BelongsTo_Should_Return_Expected_Result()
        {
            var factory = new ConvolutionFactory<double>(new DoubleAlgebraicNumberCalculator());

            var northEast = factory.CreateDirection(1.0, 1.0);
            var northWest = factory.CreateDirection(-1.0, 1.0);

            var range = factory.CreateDirectionRange(northEast, northWest, Orientation.CounterClockwise);

            const double perturbation = 1.0e-2;

            var d1 = factory.CreateDirection(1.0 + perturbation, 1.0);

            var d2 = factory.CreateDirection(1.0 - perturbation, 1.0);

            var d3 = factory.CreateDirection(-1.0 + perturbation, 1.0);

            var d4 = factory.CreateDirection(-1.0 - perturbation, 1.0);

            d1.BelongsTo(range).Should().BeFalse();
            d2.BelongsTo(range).Should().BeTrue();
            d3.BelongsTo(range).Should().BeTrue();
            d4.BelongsTo(range).Should().BeFalse();
        }
    }
}
