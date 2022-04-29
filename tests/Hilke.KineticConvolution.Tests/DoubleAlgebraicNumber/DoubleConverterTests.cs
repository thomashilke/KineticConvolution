using System;

using FluentAssertions;

using Fractions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.DoubleAlgebraicNumber
{
    [TestFixture]
    public class DoubleConverterTests
    {
        private static readonly ConvolutionFactory<double> DoubleFactory =
            new(new DoubleAlgebraicNumberCalculator());

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = new DoubleConverter<double>(DoubleFactory);

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = new DoubleConverter<double>(DoubleFactory);

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<DoubleConverter<double>>();
        }

        [Test]
        public void Given_a_number_When_calling_FromDouble_and_ToDouble_Then_convert_as_expected()
        {
            // Arrange
            var subject = new DoubleConverter<double>(DoubleFactory);
            var number = 1.4;

            // Act
            var converted = subject.FromDouble(number);
            var actual = subject.ToDouble(converted);

            // Assert
            actual.Should().Be(number);
        }

        [Test]
        public void Given_a_Point_When_calling_FromDouble_and_ToDouble_Then_convert_as_expected()
        {
            // Arrange
            var subject = new DoubleConverter<double>(DoubleFactory);
            var point = DoubleFactory.CreatePoint(1.0, 1.3);

            // Act
            var converted = subject.FromDouble(point);
            var actual = subject.ToDouble(converted);

            // Assert
            actual.Should().BeEquivalentTo(point);
        }


        [Test]
        public void Given_a_Direction_When_calling_FromDouble_and_ToDouble_Then_convert_as_expected()
        {
            // Arrange
            var subject = new DoubleConverter<double>(DoubleFactory);
            var direction = DoubleFactory.CreateDirection(1.0, 1.3);

            // Act
            var converted = subject.FromDouble(direction);
            var actual = subject.ToDouble(converted);

            // Assert
            actual.Should().BeEquivalentTo(direction);
        }

        [Test]
        public void Given_a_DirectionRange_When_calling_FromDouble_and_ToDouble_Then_convert_as_expected()
        {
            // Arrange
            var subject = new DoubleConverter<double>(DoubleFactory);
            var range = DoubleFactory.CreateDirectionRange(
                DoubleFactory.CreateDirection(1.0, 1.3),
                DoubleFactory.CreateDirection(2.0, 2.3),
                Orientation.CounterClockwise);

            // Act
            var converted = subject.FromDouble(range);
            var actual = subject.ToDouble(converted);

            // Assert
            actual.Should().BeEquivalentTo(range);
        }

        [Test]
        public void Given_a_segment_When_calling_FromDouble_and_ToDouble_Then_convert_as_expected()
        {
            // Arrange
            var subject = new DoubleConverter<double>(DoubleFactory);
            var segment = DoubleFactory.CreateSegment(
                DoubleFactory.CreatePoint(1.0, 1.3),
                DoubleFactory.CreatePoint(2.0, 2.3),
                Fraction.One);

            // Act
            var converted = subject.FromDouble(segment);
            var actual = subject.ToDouble(converted);

            // Assert
            actual.Should().BeEquivalentTo(segment);
        }

        [Test]
        public void Given_an_arc_When_calling_FromDouble_and_ToDouble_Then_convert_as_expected()
        {
            // Arrange
            var subject = new DoubleConverter<double>(DoubleFactory);
            var range = DoubleFactory.CreateDirectionRange(
                DoubleFactory.CreateDirection(1.0, 1.3),
                DoubleFactory.CreateDirection(2.0, 2.3),
                Orientation.CounterClockwise);

            var arc = DoubleFactory.CreateArc(
                DoubleFactory.CreatePoint(1.0, 1.3),
                range,
                1.4,
                Fraction.One);

            // Act
            var converted = subject.FromDouble(arc);
            var actual = subject.ToDouble(converted);

            // Assert
            actual.Should().BeEquivalentTo(arc);
        }

        [Test]
        public void Given_a_shape_When_calling_FromDouble_and_ToDouble_Then_convert_as_expected()
        {
            // Arrange
            var subject = new DoubleConverter<double>(DoubleFactory);
            var point1 = DoubleFactory.CreatePoint(1.0, 1.3);
            var point2 = DoubleFactory.CreatePoint(2.0, 2.3);
            var segment12 = DoubleFactory.CreateSegment(point1, point2, Fraction.One);
            var segment21 = DoubleFactory.CreateSegment(point2, point1, Fraction.One);
            var direction12 = point1.DirectionTo(point2);
            var range1 = DoubleFactory.CreateDirectionRange(
                direction12.NormalDirection(),
                direction12.NormalDirection().Opposite(),
                Orientation.CounterClockwise);
            var range2 = DoubleFactory.CreateDirectionRange(
                direction12.NormalDirection().Opposite(),
                direction12.NormalDirection(),
                Orientation.CounterClockwise);
            var arc1 = DoubleFactory.CreateArc(point1, range1, 0.0, Fraction.One);
            var arc2 = DoubleFactory.CreateArc(point2, range2, 0.0, Fraction.One);
            var shape = DoubleFactory.CreateShape(new Tracing<double>[] { arc1, segment12, arc2, segment21 });
            
            // Act
            var converted = subject.FromDouble(shape);
            var actual = subject.ToDouble(converted);

            // Assert
            actual.Should().BeEquivalentTo(shape);
        }
    }
}
