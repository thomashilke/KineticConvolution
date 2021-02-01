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

        // Case 1
        [Test]
        public void
            When_calling_ConvolveArcAndSegment_With_two_shapes_without_tangents_Then_the_result_Should_be_empty()    
        {
            // Arrange arc1
            var arc = _factory.CreateArc(
                radius: 2.0,
                weight: 3,
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
                endX: 15,
                endY: 10,
                weight: 4);

            // Act
            var actual = _factory.ConvolveArcAndSegment(arc, segment);

            // Assert
            actual.Should().BeEmpty();
        }

        // Case 2
        [Test]
        public void
            When_calling_ConvolveArcAndSegment_With_same_counterClockwise_orientation_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc
            var arc = _factory.CreateArc(
                radius: 2.0,
                weight: 5,
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
                weight: 20);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Segment<double>>().Should().BeEquivalentTo(expected);
        }

        // Case 3
        [Test]
        public void
            When_calling_ConvolveArcs_With_same_counterClockwise_orientation_and_r1_greater_than_r2_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc1
            var arc1 = _factory.CreateArc(
                radius: 2.0,
                weight: 2,
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
                weight: 4,
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

        // Case 4
        [Test]
        public void
            When_calling_ConvolveArcs_With_different_orientation_and_r1_smaller_than_r2_Then_the_correct_result_Should_be_returned()
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
                radius: 3.0,
                weight: 1,
                centerX: 5.0,
                centerY: 3.0,
                directionStartX: 1.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 3.0,
                orientation: Orientation.Clockwise);

            // Arrange expected
            var expected = _factory.CreateArc(
                radius: -1,
                weight: 1,
                centerX: 6,
                centerY: 6,
                directionStartX: -12,
                directionStartY: 0,
                directionEndX: 0,
                directionEndY: -3,
                orientation: Orientation.CounterClockwise);

            // Act
            var actual = _factory.ConvolveArcs(arc1, arc2);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Segment<double>>().Should().BeEquivalentTo(expected);
        }

        // Case 5
        [Test]
        public void
            When_calling_ConvolveArcs_With_opposite_orientations_and_r1_greater_than_r2_Then_the_correct_result_Should_be_returned()
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
                directionEndY: 3.0,
                orientation: Orientation.CounterClockwise);

            // Act
            var actual = _factory.ConvolveArcs(arc1, arc2);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Arc<double>>().Should().BeEquivalentTo(expected);
        }

        // Case 6
        [Test]
        public void
            When_calling_ConvolveArcs_With_same_clockwise_orientation_and_r1_greater_than_r2_Then_the_correct_result_Should_be_returned()
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
                directionStartX: 12.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 51.0,
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

        // Case 7
        [Test]
        public void
            When_calling_ConvolveArcs_With_opposite_orientation_and_r1_equals_r2_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc1
            var arc1 = _factory.CreateArc(
                radius: 2.0,
                weight: 5,
                centerX: 1.0,
                centerY: 3.0,
                directionStartX: 2.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 5.0,
                orientation: Orientation.CounterClockwise);

            // Arrange arc2
            var arc2 = _factory.CreateArc(
                radius: 2.0,
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
                radius: 0.0,
                weight: 10,
                centerX: 6.0,
                centerY: 6.0,
                directionStartX: 12.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 3.0,
                orientation: Orientation.CounterClockwise);

            // Act
            var actual = _factory.ConvolveArcs(arc1, arc2);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Convolution.As<Arc<double>>().Should().BeEquivalentTo(expected);
        }

        // Case 8
        [Test]
        public void
            When_calling_ConvolveArcs_With_same_counterClockwise_orientation_and_r1_smaller_than_r2_Then_the_correct_result_Should_be_returned()
        {
            // Arrange arc1
            var arc1 = _factory.CreateArc(
                radius: 1.0,
                weight: 2,
                centerX: 1.0,
                centerY: 3.0,
                directionStartX: 2.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 5.0,
                orientation: Orientation.CounterClockwise);

            // Arrange arc2
            var arc2 = _factory.CreateArc(
                radius: 2.0,
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
                weight: 4,
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

        // Case 9
        [Test]
        public void When_calling_Convolve_With_two_segment_with_same_orientation_The_result_should_be_correct()
        {
            // Arrange segment
            var segment1 = _factory.CreateSegment(
                startX: 10,
                startY: 5,
                endX: 10,
                endY: 10,
                weight: 4);

            // Arrange segment
            var segment2 = _factory.CreateSegment(
                startX: -1,
                startY: 5,
                endX: -1,
                endY: 10,
                weight: 4);

            // Act
            var actual = _factory.Convolve(segment1, segment2);

            // Assert
            actual.Should().BeEmpty();
        }

        // Case 10
        [Test]
        public void When_calling_Convolve_With_two_segment_with_opposite_orientation_The_result_should_be_correct()
        {
            // Arrange segment
            var segment1 = _factory.CreateSegment(
                startX: 10,
                startY: 5,
                endX: 10,
                endY: 10,
                weight: 4);

            // Arrange segment
            var segment2 = _factory.CreateSegment(
                startX: -1,
                startY: 10,
                endX: -1,
                endY: 5,
                weight: 4);

            // Act
            var actual = _factory.Convolve(segment1, segment2);

            // Assert
            actual.Should().BeEmpty();
        }
    }
}
