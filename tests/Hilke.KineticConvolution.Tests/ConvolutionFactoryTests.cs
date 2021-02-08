using System;
using System.Linq;

using FluentAssertions;

using Fractions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class ConvolutionFactoryTests
    {
        private ConvolutionFactory<double> _factory;
        private DirectionRange<double> _range;

        [SetUp]
        public void SetUp()
        {
            var calculator = new DoubleAlgebraicNumberCalculator();
            _factory = new ConvolutionFactory<double>(calculator);

            _range = _factory.CreateDirectionRange(
                _factory.CreateDirection(3.0, 0.0),
                _factory.CreateDirection(0.0, 2.0),
                Orientation.CounterClockwise);
        }

        [Test]
        public void When_calling_CreateSegment_With_null_start_point_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            var end = _factory.CreatePoint(10.0, 12.0);

            // Act
            Action action = () => _factory.CreateSegment(null!, end, new Fraction(3, 2));

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("start");
        }

        [Test]
        public void When_calling_CreateSegment_With_null_end_point_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            var start = _factory.CreatePoint(10.0, 12.0);

            // Act
            Action action = () => _factory.CreateSegment(start, null!, new Fraction(3, 2));

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName
                  .Should().Be("end");
        }

        [Test]
        public void When_calling_CreateSegment_With_two_points_with_same_values_Then_an_ArgumentException_Should_be_thrown()
        {
            // Arrange
            var start = _factory.CreatePoint(10.0, 12.0);
            var end = _factory.CreatePoint(10.0, 12.0);

            // Act
            Action action = () => _factory.CreateSegment(start, end, new Fraction(3, 2));

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentException>()
                  .WithMessage("The start point cannot be the same as end point. (Parameter 'start')")
                  .And.ParamName
                  .Should().Be("start");
        }

        [Test]
        public void When_calling_CreateArc_With_null_center_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Act
            Action action = () => _factory.CreateArc(
                new Fraction(3, 2),
                null!,
                _range,
                5.0);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("center");
        }

        [Test]
        public void When_calling_CreateArc_With_null_direction_range_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Act
            Action action = () => _factory.CreateArc(
                new Fraction(3, 2),
                _factory.CreatePoint(10.0, 5.0),
                null!,
                5.0);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("directions");
        }

        [Test]
        public void When_calling_ConvolveShapes_With_null_shapes_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            var segment = _factory.CreateSegment(new Fraction(1, 2), 0.0, 0.0, 10.0, 0.0);
            var shape = new Shape<double>(new[] {segment});
            // Act
            Action action1 = () => _factory.ConvolveShapes(null!, shape);
            Action action2 = () => _factory.ConvolveShapes(shape, null!);

            // Assert
            action1.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be("shape1");

            action2.Should()
                   .ThrowExactly<ArgumentNullException>()
                   .And.ParamName.Should()
                   .Be("shape2");
        }

        [Test]
        public void When_calling_ConvolveShapes_With_valid_shapes_Then_the_correct_result_Should_be_returned()
        {
            // Arrange
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

            var segment = _factory.CreateSegment(
                startX: 10,
                startY: 5,
                endX: 10,
                endY: 10,
                weight: 4);

            var expected = _factory.CreateSegment(
                startX: 13,
                startY: 8,
                endX: 13,
                endY: 13,
                weight: 20);

            var shape1 = new Shape<double>(new[] { arc });
            var shape2 = new Shape<double>(new[] { segment });

            // Act
            var actual = _factory.ConvolveShapes(shape1, shape2);

            // Assert
            actual.ConvolvedTracings.Should().HaveCount(1);

            actual.ConvolvedTracings
                  .Single()
                  .Convolution
                  .As<Segment<double>>()
                  .Should()
                  .BeEquivalentTo(expected);
        }

        [Test]
        public void When_calling_CreateShape_With_null_tracings_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _factory.CreateShape(null!);

            // Assert
            action.Should()
                   .ThrowExactly<ArgumentNullException>()
                   .And.ParamName.Should()
                   .Be("tracings");
        }

        [Test]
        public void When_calling_CreateShape_With_empty_tracings_enumerable_Then_an_ArgumentException_Should_be_thrown()
        {
            // Arrange
            var shapes = Enumerable.Empty<Tracing<double>>();
            Action action = () => _factory.CreateShape(shapes);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentException>()
                  .WithMessage("There should be at least one tracing. (Parameter 'tracings')")
                  .And.ParamName.Should()
                  .Be("tracings");
        }

        [Test]
        public void When_calling_CreateShape_With_non_G1_continuous_tracings_enumerable_Then_an_ArgumentException_Should_be_thrown()
        {
            // Arrange
            var segment1 = _factory.CreateSegment(new Fraction(2, 3), 0.0, 0.0, 10.0, 0.0);
            var segment2 = _factory.CreateSegment(new Fraction(5, 3), 10.0, 0.0, 0.0, 10.0);

            var shapes = new []{segment1, segment2};
            Action action = () => _factory.CreateShape(shapes);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentException>()
                  .WithMessage("The tracings should be continuous. (Parameter 'tracings')")
                  .And.ParamName.Should()
                  .Be("tracings");
        }

        [Test]
        public void When_calling_CreateShape_With_G1_continuous_tracings_enumerable_Then_it_Should_not_throw_any_Exception()
        {
            // Arrange
            var arc1 = _factory.CreateArc(
                radius: 5.0,
                weight: 3,
                centerX: 0.0,
                centerY: 0.0,
                directionStartX: 0.0,
                directionStartY: -2.0,
                directionEndX: 0.0,
                directionEndY: 5.0,
                orientation: Orientation.Clockwise);

            var arc2 = _factory.CreateArc(
                radius: 5.0,
                weight: 3,
                centerX: 10.0,
                centerY: 0.0,
                directionStartX: 0.0,
                directionStartY: 3.0,
                directionEndX: 0.0,
                directionEndY: -10.0,
                orientation: Orientation.Clockwise);

            var segment1 = _factory.CreateSegment(new Fraction(1, 3), 0.0, 5.0, 10.0, 5.0);
            var segment2 = _factory.CreateSegment(new Fraction(2, 3), 10.0, -5.0, 0.0, -5.0);

            var shapes = new Tracing<double>[] { arc1, segment1, arc2, segment2};
            Action action = () => _factory.CreateShape(shapes);

            // Assert
            action.Should()
                  .NotThrow();
        }

        // Case 1 === Test cases ===
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

            // Arrange expected
            var expected = _factory.CreateSegment(
                startX: 13,
                startY: 8,
                endX: 13,
                endY: 13,
                weight: 20);

            // Act
            var actual = _factory.ConvolveArcAndSegment(arc, segment);


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
                radius: 3.0,
                weight: 3,
                centerX: 5.0,
                centerY: 3.0,
                directionStartX: 1.0,
                directionStartY: 0.0,
                directionEndX: 0.0,
                directionEndY: 3.0,
                orientation: Orientation.Clockwise);

            // Arrange expected
            var expected = _factory.CreateArc(
                radius: 1,
                weight: 6,
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
            actual.Single().Convolution.As<Arc<double>>().Should().BeEquivalentTo(expected);
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
            // Arrange segment 1
            var segment1 = _factory.CreateSegment(
                startX: 10,
                startY: 5,
                endX: 10,
                endY: 10,
                weight: 4);

            // Arrange segment 2
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
