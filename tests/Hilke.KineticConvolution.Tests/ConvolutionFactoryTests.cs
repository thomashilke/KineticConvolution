using System.Linq;

using NUnit.Framework;
using FluentAssertions;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class ConvolutionFactoryTests
    {
        private ConvolutionFactory<double> _factory;

        [SetUp]
        public void SetUp()
        {
            var calculator = new DoubleAlgebraicNumberCalculator();
            _factory = new ConvolutionFactory<double>(calculator);
        }

        [Test]
        public void When_calling_ConvolveArcs_With_same_counterClockwise_orientation_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc1
            var start1 = _factory.CreateDirection(2.0, 0.0);
            var end1 = _factory.CreateDirection(0.0, 5.0);
            var range1 = _factory.CreateDirectionRange(start1, end1, Orientation.CounterClockwise);

            var arc1 = _factory.CreateArc(
                weight: 1,
                center: _factory.CreatePoint(1.0, 3.0),
                directions: range1,
                radius: 2.0);

            // Arrange arc2
            var start2 = _factory.CreateDirection(1.0, 0.0);
            var end2 = _factory.CreateDirection(0.0, 3.0);
            var range2 = _factory.CreateDirectionRange(start2, end2, Orientation.CounterClockwise);

            var arc2 = _factory.CreateArc(
                weight: 2,
                center: _factory.CreatePoint(5.0, 3.0),
                directions: range2,
                radius: 1.0);

            var expectedStart = _factory.CreateDirection(12.0, 0.0);
            var expectedEnd = _factory.CreateDirection(0.0, 51.0);

            var expectedRange = _factory.CreateDirectionRange(start2, end2, Orientation.CounterClockwise);
            var expected = _factory.CreateArc(
                weight: 2,
                center: _factory.CreatePoint(6.0, 6.0),
                directions: expectedRange,
                radius: 3.0);


            // Act
            var actual = _factory.ConvolveArcs(arc1, arc2);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Arc<double>>().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void When_calling_ConvolveArcs_With_opposite_orientations_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc1
            var start1 = _factory.CreateDirection(2.0, 0.0);
            var end1 = _factory.CreateDirection(0.0, 5.0);
            var range1 = _factory.CreateDirectionRange(start1, end1, Orientation.CounterClockwise);

            var arc1 = _factory.CreateArc(
                weight: 1,
                center: _factory.CreatePoint(1.0, 3.0),
                directions: range1,
                radius: 2.0);

            // Arrange arc2
            var start2 = _factory.CreateDirection(1.0, 0.0);
            var end2 = _factory.CreateDirection(0.0, 3.0);
            var range2 = _factory.CreateDirectionRange(start2, end2, Orientation.Clockwise);

            var arc2 = _factory.CreateArc(
                weight: 2,
                center: _factory.CreatePoint(5.0, 3.0),
                directions: range2,
                radius: 1.0);

            var expectedStart = _factory.CreateDirection(-12.0, 0.0);
            var expectedEnd = _factory.CreateDirection(0.0, -31.0);

            var expectedRange = _factory.CreateDirectionRange(start2, end2, Orientation.CounterClockwise);
            var expected = _factory.CreateArc(
                weight: 2,
                center: _factory.CreatePoint(6.0, 6.0),
                directions: expectedRange,
                radius: 1.0);


            // Act
            var actual = _factory.ConvolveArcs(arc1, arc2);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Arc<double>>().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void When_calling_ConvolveArcs_With_same_clockwise_orientation_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc1
            var start1 = _factory.CreateDirection(2.0, 0.0);
            var end1 = _factory.CreateDirection(0.0, 5.0);
            var range1 = _factory.CreateDirectionRange(start1, end1, Orientation.Clockwise);

            var arc1 = _factory.CreateArc(
                weight: 1,
                center: _factory.CreatePoint(1.0, 3.0),
                directions: range1,
                radius: 2.0);

            // Arrange arc2
            var start2 = _factory.CreateDirection(1.0, 0.0);
            var end2 = _factory.CreateDirection(0.0, 3.0);
            var range2 = _factory.CreateDirectionRange(start2, end2, Orientation.Clockwise);

            var arc2 = _factory.CreateArc(
                weight: 2,
                center: _factory.CreatePoint(5.0, 3.0),
                directions: range2,
                radius: 1.0);

            var expectedStart = _factory.CreateDirection(12.0, 0.0);
            var expectedEnd = _factory.CreateDirection(0.0, 51.0);

            var expectedRange = _factory.CreateDirectionRange(start2, end2, Orientation.Clockwise);
            var expected = _factory.CreateArc(
                weight: 2,
                center: _factory.CreatePoint(6.0, 6.0),
                directions: expectedRange,
                radius: 3.0);

            // Act
            var actual = _factory.ConvolveArcs(arc1, arc2);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Arc<double>>().Should().BeEquivalentTo(expected);
        }
    }
}
