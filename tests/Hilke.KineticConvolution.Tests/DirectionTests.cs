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

        [Test]
        public void When_Direction_Is_One_DirectionRange_Extremity_Then_Direction_Should_Not_Belongs_Strictly_To_DirectionRange()
        {
            var factory = new ConvolutionFactory();

            var east = factory.CreateDirection(1.0, 0.0);
            var north = factory.CreateDirection(0.0, 1.0);

            var directionRange1 = factory.CreateDirectionRange(east, north, Orientation.CounterClockwise);
            var directionRange2 = factory.CreateDirectionRange(east, north, Orientation.Clockwise);

            east.BelongsTo(directionRange1).Should().BeTrue();
            east.BelongsTo(directionRange2).Should().BeTrue();
            north.BelongsTo(directionRange1).Should().BeTrue();
            north.BelongsTo(directionRange2).Should().BeTrue();

            east.StrictlyBelongsTo(directionRange1).Should().BeFalse();
            east.StrictlyBelongsTo(directionRange2).Should().BeFalse();
            north.StrictlyBelongsTo(directionRange1).Should().BeFalse();
            north.StrictlyBelongsTo(directionRange2).Should().BeFalse();
        }
    }
}
