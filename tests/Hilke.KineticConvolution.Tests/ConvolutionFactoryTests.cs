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

            // Arrange expected
            var expected = _factory.CreateSegment(
                startX: 13,
                startY: 8,
                endX: 13,
                endY: 13,
                weight: 10);

            // Act
            var actual = _factory.ConvolveArcAndSegment(arc, segment);

            // Assert
            actual.Should().HaveCount(1);
            actual.Single().Should().BeOfType<Segment<double>>().And.BeEquivalentTo(expected);
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
            actual.Single().Parent1.Should().BeSameAs(arc1);
            actual.Single().Parent2.Should().BeSameAs(arc2);
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
            actual.Single().Parent1.Should().BeSameAs(arc1);
            actual.Single().Parent2.Should().BeSameAs(arc2);
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
            actual.Single().Parent1.Should().BeSameAs(arc1);
            actual.Single().Parent2.Should().BeSameAs(arc2);
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
            actual.Single().Parent1.Should().BeSameAs(arc1);
            actual.Single().Parent2.Should().BeSameAs(arc2);
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
            actual.Single().Parent1.Should().BeSameAs(arc1);
            actual.Single().Parent2.Should().BeSameAs(arc2);
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
            actual.Single().Parent1.Should().BeSameAs(arc1);
            actual.Single().Parent2.Should().BeSameAs(arc2);
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

        [Test]
        public void When_Two_Shapes_Are_Convolved_Then_Parent1_Should_Belong_To_Shape1_And_Parent2_Should_Belong_To_Shape2()
        {
            // Arrange
            var shape1 = createShape1();
            var shape2 = createShape2();

            // Act
            var convolution = _factory.ConvolveShapes(shape1, shape2);

            // Assert
            convolution.ConvolvedTracings.ToList()
                .ForEach(convolvedTracing =>
                    shape1.Tracings.Contains(convolvedTracing.Parent1).Should().BeTrue());

            convolution.ConvolvedTracings.ToList()
                .ForEach(convolvedTracing =>
                    shape2.Tracings.Contains(convolvedTracing.Parent2).Should().BeTrue());

            Shape<double> createShape1()
            {
                var weight = new Fraction(1, 2);

                var d1 = _factory.CreateDirection(-1, 2);
                var d2 = _factory.CreateDirection(-1, -2);
                var d3 = _factory.CreateDirection(2, 0);

                var c1 = _factory.CreatePoint(1, 0);
                var c2 = _factory.CreatePoint(0, 2);
                var c3 = _factory.CreatePoint(-1, 0);

                var range1 = _factory.CreateDirectionRange(
                    d3.NormalDirection().Opposite(),
                    d1.NormalDirection().Opposite(),
                    Orientation.CounterClockwise);

                var range2 = _factory.CreateDirectionRange(
                    d1.NormalDirection().Opposite(),
                    d2.NormalDirection().Opposite(),
                    Orientation.CounterClockwise);

                var range3 = _factory.CreateDirectionRange(
                    d2.NormalDirection().Opposite(),
                    d3.NormalDirection().Opposite(),
                    Orientation.CounterClockwise);

                var arc1 = _factory.CreateArc(c1, range1, 1.0, weight);
                var arc2 = _factory.CreateArc(c2, range2, 1.0, weight);
                var arc3 = _factory.CreateArc(c3, range3, 1.0, weight);

                var segment1 = _factory.CreateSegment(arc1.End, arc2.Start, weight);
                var segment2 = _factory.CreateSegment(arc2.End, arc3.Start, weight);
                var segment3 = _factory.CreateSegment(arc3.End, arc1.Start, weight);

                return _factory.CreateShape(
                    new Tracing<double>[]
                    {
                        arc1, segment1, arc2, segment2, arc3, segment3
                    });
            }

            Shape<double> createShape2()
            {
                var weight = new Fraction(1, 2);

                var d1 = _factory.CreateDirection(0, 3);
                var d2 = _factory.CreateDirection(-3, 0);
                var d3 = _factory.CreateDirection(0, -3);
                var d4 = _factory.CreateDirection(3, 0);

                var c1 = _factory.CreatePoint(3, 0);
                var c2 = _factory.CreatePoint(3, 3);
                var c3 = _factory.CreatePoint(0, 3);
                var c4 = _factory.CreatePoint(0, 0);

                var range1 = _factory.CreateDirectionRange(
                    d4.NormalDirection().Opposite(),
                    d1.NormalDirection().Opposite(),
                    Orientation.CounterClockwise);

                var range2 = _factory.CreateDirectionRange(
                    d1.NormalDirection().Opposite(),
                    d2.NormalDirection().Opposite(),
                    Orientation.CounterClockwise);

                var range3 = _factory.CreateDirectionRange(
                    d2.NormalDirection().Opposite(),
                    d3.NormalDirection().Opposite(),
                    Orientation.CounterClockwise);

                var range4 = _factory.CreateDirectionRange(
                    d3.NormalDirection().Opposite(),
                    d4.NormalDirection().Opposite(),
                    Orientation.CounterClockwise);

                var arc1 = _factory.CreateArc(c1, range1, 1.0, weight);
                var arc2 = _factory.CreateArc(c2, range2, 1.0, weight);
                var arc3 = _factory.CreateArc(c3, range3, 1.0, weight);
                var arc4 = _factory.CreateArc(c4, range4, 1.0, weight);

                var segment1 = _factory.CreateSegment(arc1.End, arc2.Start, weight);
                var segment2 = _factory.CreateSegment(arc2.End, arc3.Start, weight);
                var segment3 = _factory.CreateSegment(arc3.End, arc4.Start, weight);
                var segment4 = _factory.CreateSegment(arc4.End, arc1.Start, weight);

                return _factory.CreateShape(
                    new Tracing<double>[]
                    {
                        arc1, segment1, arc2, segment2, arc3, segment3, arc4, segment4
                    });
            }
        }

        [Test]
        public void When_A_Shape_Is_A_Full_Disk_Then_Convolve_Should_Return_Correct_Number_Of_Convolved_Tracings()
        {
            // Arrange
            var eastDirection = _factory.CreateDirection(1.0, 0.0);

            var diskArc = _factory.CreateArc(
                center: _factory.CreatePoint(0.0, 0.0),
                directions: _factory.CreateDirectionRange(eastDirection, eastDirection, Orientation.CounterClockwise),
                radius: 1.0,
                weight: 1);

            var origin = _factory.CreatePoint(0.0, 0.0);

            var direction = _factory.CreateDirection(1.0, 2.0).NormalDirection().Opposite();

            var smoothingArc1 = _factory.CreateArc(
                origin,
                _factory.CreateDirectionRange(
                    direction.Opposite(),
                    direction,
                    Orientation.CounterClockwise),
                0.0,
                1);

            var smoothingArc2 = _factory.CreateArc(
                origin,
                _factory.CreateDirectionRange(
                    direction,
                    direction.Opposite(),
                    Orientation.CounterClockwise),
                0.0,
                1);

            var verticalSegment = _factory.CreateSegment(origin, _factory.CreatePoint(0.0, 1.0), 1);

            // Act
            var convolution1 = _factory.Convolve(diskArc, smoothingArc1);
            var convolution2 = _factory.Convolve(diskArc, smoothingArc2);
            var convolution3 = _factory.Convolve(diskArc, verticalSegment);

            // Assert
            convolution1.Should().HaveCount(1);
            convolution2.Should().HaveCount(2);
            convolution3.Should().HaveCount(1);

            convolution1.Select(convolution => convolution.Convolution).Should().AllBeOfType<Arc<double>>();
            convolution2.Select(convolution => convolution.Convolution).Should().AllBeOfType<Arc<double>>();
            convolution3.Single().Convolution.Should().BeOfType(typeof(Segment<double>));
            convolution3.Single().Convolution.Weight.Should().Be(1);
        }
    }
}
