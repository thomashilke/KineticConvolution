using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionTests
    {
        [Test]
        public void When_Direction_Is_Given_Then_It_Should_Belongs_To_The_Expected_Half_Plan()
        {
            var factory = new ConvolutionFactory();

            var east = factory.CreateDirection(3.0, 0.0);
            var west = factory.CreateDirection(-2.0, 0.0);

            var lowerHalfPlan = factory.CreateDirectionRange(east, west, Orientation.Clockwise);
            var upperHalfPlan = factory.CreateDirectionRange(east, west, Orientation.CounterClockwise);

            var directionInUpperHalfPlan = factory.CreateDirection(-5.0, 0.5);
            var directionInLowerHalfPlan = factory.CreateDirection(-5.0, -0.5);

            directionInUpperHalfPlan.BelongsTo(lowerHalfPlan).Should().BeFalse();
            directionInUpperHalfPlan.BelongsTo(upperHalfPlan).Should().BeTrue();

            directionInLowerHalfPlan.BelongsTo(lowerHalfPlan).Should().BeTrue();
            directionInLowerHalfPlan.BelongsTo(upperHalfPlan).Should().BeFalse();
        }
    }
}
