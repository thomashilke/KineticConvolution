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
            var arc1 = _factory.CreateArc(
                radius: 2.0,
                weight: 1,
                centerX: 1.0,
                centerY: 3.0,
                directionStartX: 2.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 5.0,
                orientation: Orientation.CounterClockwise);

            // Arrange arc2
            var arc2 = _factory.CreateArc(
                radius: 1.0,
                weight: 2,
                centerX: 5.0,
                centerY: 3.0,
                directionStartX: 1.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 3.0,
                orientation: Orientation.CounterClockwise);

            // Arrange expected
            var expected = _factory.CreateArc(
                radius: 3.0,
                weight: 2,
                centerX: 6.0,
                centerY: 6.0,
                directionStartX: 12.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 51.0,
                orientation: Orientation.CounterClockwise);

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
            var arc1 = _factory.CreateArc(
                radius: 2.0,
                weight: 1,
                centerX: 1.0,
                centerY: 3.0,
                directionStartX: 2.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 5.0,
                orientation: Orientation.CounterClockwise);

            // Arrange arc2
            var arc2 = _factory.CreateArc(
                radius: 1.0,
                weight: 2,
                centerX: 5.0,
                centerY: 3.0,
                directionStartX: 1.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 3.0,
                orientation: Orientation.Clockwise);

            // Arrange expected
            var expected = _factory.CreateArc(
                radius: 1.0,
                weight: 2,
                centerX: 6.0,
                centerY: 6.0,
                directionStartX: 12.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 31.0,
                orientation: Orientation.CounterClockwise);

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
            var arc1 = _factory.CreateArc(
                radius: 2.0,
                weight: 1,
                centerX: 1.0,
                centerY: 3.0,
                directionStartX: 2.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 5.0,
                orientation: Orientation.Clockwise);

            // Arrange arc2
            var arc2 = _factory.CreateArc(
                radius: 1.0,
                weight: 2,
                centerX: 5.0,
                centerY: 3.0,
                directionStartX: 1.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 3.0,
                orientation: Orientation.Clockwise);

            // Arrange expected
            var expected = _factory.CreateArc(
                radius: 3.0,
                weight: 2,
                centerX: 6.0,
                centerY: 6.0,
                directionStartX: 12.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 51.0,
                orientation: Orientation.Clockwise);

            // Act
            var actual = _factory.ConvolveArcs(arc1, arc2);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Arc<double>>().Should().BeEquivalentTo(expected);
        }

        [Test]
        public void When_calling_ConvolveArcAndSegment_With_same_counterClockwise_orientation_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc
            var arc = _factory.CreateArc(
                radius: 2.0,
                weight: 1,
                centerX: 1.0,
                centerY: 3.0,
                directionStartX: 2.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 5.0,
                orientation: Orientation.CounterClockwise);

            // Arrange segment
            var segment = _factory.CreateSegment(
                startX: 10,
                startY: 5,
                endX: 10,
                endY: 10,
                weight: 4);

            // Act
            var actual = _factory.ConvolveArcAndSegment(arc, segment);
            var expected = _factory.CreateSegment(
                startX: 13,
                startY: 8,
                endX: 13,
                endY: 13,
                weight: 4);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Segment<double>>().Should().BeEquivalentTo(expected);
        }
    }
}
