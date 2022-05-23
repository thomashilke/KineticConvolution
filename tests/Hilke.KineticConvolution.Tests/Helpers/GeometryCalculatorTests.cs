using System;
using System.Collections.Generic;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;
using Hilke.KineticConvolution.Helpers;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.Helpers
{
    [TestFixture(TestOf = typeof(GeometryCalculator<>))]
    internal class GeometryCalculatorTests
    {

        private static readonly DoubleAlgebraicNumberCalculator DoubleCalculator = new();

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = new GeometryCalculator<double>(DoubleCalculator);

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = new GeometryCalculator<double>(DoubleCalculator);

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<GeometryCalculator<double>>();
        }

        [Test]
        public void Given_invalid_parameters_When_calling_constructor_Then_throw_exception()
        {
            // Arrange
            Action action = () => _ = new GeometryCalculator<double>(calculator: null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("calculator");
        }
    }
}


