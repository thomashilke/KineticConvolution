using System;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.DoubleAlgebraicNumber
{
    [TestFixture]
    public class DoubleAlgebraicNumberCalculatorTests
    {
        private DoubleAlgebraicNumberCalculator _calculator;

        [SetUp]
        public void SetUp() =>
            _calculator = new DoubleAlgebraicNumberCalculator(1e-12);

        [Test]
        public void When_calling_DoubleAlgebraicNumberCalculator_With_negative_tolerance_Then_an_ArgumentException_Should_be_thrown()
        {
            // Arrange
            const double negativeTolerance = -1e-15;

            Action action = () => _ = new DoubleAlgebraicNumberCalculator(negativeTolerance);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentException>()
                  .WithMessage("The zero tolerance must be positive, but got '-1E-15'. (Parameter 'zeroTolerance')");
        }

        [Test]
        public void When_calling_Opposite_of_a_valid_number_Then_the_correct_result_Should_be_returned()
        {
            // Arrange
            const double number = 10.5;

            // Assert
            _calculator.Opposite(number).Should().Be(-number);
        }

        [Test]
        public void When_calling_Inverse_of_a_non_null_number_Then_the_correct_result_Should_be_returned()
        {
            // Arrange
            const double number = 2.0;
            const double inverse = 1.0 / number;

            // Assert
            _calculator.Inverse(number).Should().Be(inverse);
        }
    }
}
