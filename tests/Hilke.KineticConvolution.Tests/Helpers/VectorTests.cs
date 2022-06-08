using System;

using FluentAssertions;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Helpers.Tests
{
    [TestFixture(TestOf = typeof(Vector<>))]
    internal class VectorTests
    {
        private static readonly double[] ValidCoordinates = new[] { 0.0, 1.0 };

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = Vector<double>.FromEnumerable(ValidCoordinates);

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = Vector<double>.FromEnumerable(ValidCoordinates);

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<Vector<double>>();
        }

        [Test]
        public void Given_invalid_parameters_When_calling_constructor_Then_throw_exception()
        {
            // Arrange
            Action action = () => _ = Vector<double>.FromEnumerable(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("coordinates");
        }

        [Test]
        public void When_calling_properties_Should_do_it_as_expected()
        {
            // Arrange
            var subject = Vector<double>.FromEnumerable(ValidCoordinates);
            var expectedDimension = ValidCoordinates.Length;
            var expectedCoordinates = ValidCoordinates;

            // Assert
            subject.Dimension.Should().Be(expectedDimension);
            subject.Coordinates.Should().BeEquivalentTo(expectedCoordinates);
        }
    }
}
