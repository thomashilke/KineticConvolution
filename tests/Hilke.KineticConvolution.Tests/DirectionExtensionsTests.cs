using FluentAssertions;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionExtensionsTests
    {
        [Test]
        public void When_Direction_Is_Given_Then_BelongsTo_Should_Return_Expected_Result()
        {
            var east = new Direction<DoubleNumber>(DoubleNumber.FromDouble(1.0), DoubleNumber.FromDouble(0.0));
            var north = new Direction<DoubleNumber>(DoubleNumber.FromDouble(0.0), DoubleNumber.FromDouble(1.0));

            var clockwiseRange = new DirectionRange<DoubleNumber>(east, north, Orientation.Clockwise);
            var counterClockwiseRange = new DirectionRange<DoubleNumber>(east, north, Orientation.CounterClockwise);

            var northEast = new Direction<DoubleNumber>(DoubleNumber.FromDouble(1.0), DoubleNumber.FromDouble(1.0));
            var southWest = northEast.Opposite();

            northEast.BelongsTo(clockwiseRange).Should().BeFalse();
            northEast.BelongsTo(counterClockwiseRange).Should().BeTrue();

            southWest.BelongsTo(clockwiseRange).Should().BeTrue();
            southWest.BelongsTo(counterClockwiseRange).Should().BeFalse();
        }

        [Test]
        public void When_Direction_Is_Close_To_Range_Boundary_Then_BelongsTo_Should_Return_Expected_Result()
        {
            var northEast = new Direction<DoubleNumber>(DoubleNumber.FromDouble(1.0), DoubleNumber.FromDouble(1.0));
            var northWest = new Direction<DoubleNumber>(DoubleNumber.FromDouble(-1.0), DoubleNumber.FromDouble(1.0));

            var range = new DirectionRange<DoubleNumber>(northEast, northWest, Orientation.CounterClockwise);

            var perturbation = 1.0e-2;

            var d1 = new Direction<DoubleNumber>(
                DoubleNumber.FromDouble(1.0 + perturbation),
                DoubleNumber.FromDouble(1.0));

            var d2 = new Direction<DoubleNumber>(
                DoubleNumber.FromDouble(1.0 - perturbation),
                DoubleNumber.FromDouble(1.0));

            var d3 = new Direction<DoubleNumber>(
                DoubleNumber.FromDouble(-1.0 + perturbation),
                DoubleNumber.FromDouble(1.0));

            var d4 = new Direction<DoubleNumber>(
                DoubleNumber.FromDouble(-1.0 - perturbation),
                DoubleNumber.FromDouble(1.0));

            d1.BelongsTo(range).Should().BeFalse();
            d2.BelongsTo(range).Should().BeTrue();
            d3.BelongsTo(range).Should().BeTrue();
            d4.BelongsTo(range).Should().BeFalse();
        }
    }
}
