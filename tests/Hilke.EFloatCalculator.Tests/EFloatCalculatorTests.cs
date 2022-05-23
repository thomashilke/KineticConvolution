using System;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

using PeterO.Numbers;

namespace Hilke.EFloatCalculator.Tests
{
    [TestFixture(TestOf = typeof(EFloatCalculator))]
    public class EFloatCalculatorTests
    {
        private static readonly EContext ValidContext = EContext.Binary64;
        private static readonly EFloat ValidTolerance = EFloat.Create(2, -32);

        private static readonly DoubleAlgebraicNumberCalculator DoubleCalculator = new();

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = new EFloatCalculator(ValidContext, ValidTolerance);

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = new EFloatCalculator(ValidContext, ValidTolerance);

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<EFloatCalculator>();
        }

        [Test]
        public void Given_invalid_parameters_When_calling_constructor_Then_throw_exception()
        {
            // Arrange
            Action action1 = () => _ = new EFloatCalculator(context: null, ValidTolerance);
            Action action2 = () => _ = new EFloatCalculator(ValidContext, tolerance: null);
            Action action3 = () => _ = new EFloatCalculator(ValidContext, tolerance: EFloat.Create(-2, 8));

            // Assert
            action1.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("context");
            action2.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("tolerance");
            action3.Should().ThrowExactly<ArgumentOutOfRangeException>().And.ParamName.Should().Be("tolerance");
        }

        [Test]
        public void Given_invalid_arguments_When_calculate_Then_throw()
        {
            // Arrange
            var subject = new EFloatCalculator(ValidContext, ValidTolerance);
            var validNumber = EFloat.One;

            Action action1 = () => subject.Add(left: null, validNumber);
            Action action2 = () => subject.Add(validNumber, right: null);

            Action action3 = () => subject.Subtract(left: null, validNumber);
            Action action4 = () => subject.Subtract(validNumber, right: null);

            Action action5 = () => subject.Multiply(left: null, validNumber);
            Action action6 = () => subject.Multiply(validNumber, right: null);

            Action action7 = () => subject.Divide(dividend: null, validNumber);
            Action action8 = () => subject.Divide(validNumber, divisor: null);

            Action action9 = () => subject.Inverse(number: null);
            Action action10 = () => subject.Opposite(number: null);
            Action action11 = () => subject.Sign(number: null);
            Action action12 = () => subject.SquareRoot(number: null);

            // Assert
            action1.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("left");
            action2.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("right");

            action3.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("left");
            action4.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("right");

            action5.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("left");
            action6.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("right");

            action7.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("dividend");
            action8.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("divisor");

            action9.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("number");
            action10.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("number");
            action11.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("number");
            action12.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("number");
        }

        [TestCase(2.3, 4.1, TestName = "testcase 01")]
        [TestCase(2.3e-20, -4.1, TestName = "testcase 02")]
        [TestCase(0.111112, 1.0e-12, TestName = "testcase 03")]
        public void Given_pair_of_doubles_When_Add_Then_result_is_the_same_as_with_DoubleCalculator(
            double left,
            double right)
        {
            // Arrange
            var subject = new EFloatCalculator(EContext.Binary64, EFloat.Create(2, -32));
            var eLeft = EFloat.FromDouble(left);
            var eRight = EFloat.FromDouble(right);
            var expected = DoubleCalculator.Add(left, right);

            // Act
            var actual = subject.Add(eLeft, eRight).ToDouble();

            // Assert
            actual.Should().Be(expected);
        }

        [TestCase(2.3, 4.1, TestName = "testcase 01")]
        [TestCase(2.3e-20, -4.1, TestName = "testcase 02")]
        [TestCase(0.111112, 1.0e-12, TestName = "testcase 03")]
        public void Given_pair_of_doubles_When_Subtract_Then_result_is_the_same_as_with_DoubleCalculator(
            double left,
            double right)
        {
            // Arrange
            var subject = new EFloatCalculator(EContext.Binary64, EFloat.Create(2, -32));
            var eLeft = EFloat.FromDouble(left);
            var eRight = EFloat.FromDouble(right);
            var expected = DoubleCalculator.Subtract(left, right);

            // Act
            var actual = subject.Subtract(eLeft, eRight).ToDouble();

            // Assert
            actual.Should().Be(expected);
        }

        [TestCase(2.3, 4.1, TestName = "testcase 01")]
        [TestCase(2.3e-20, -4.1, TestName = "testcase 02")]
        [TestCase(0.111112, 1.0e-12, TestName = "testcase 03")]
        public void Given_pair_of_doubles_When_Multiply_Then_result_is_the_same_as_with_DoubleCalculator(
            double left,
            double right)
        {
            // Arrange
            var subject = new EFloatCalculator(EContext.Binary64, EFloat.Create(2, -32));
            var eLeft = EFloat.FromDouble(left);
            var eRight = EFloat.FromDouble(right);
            var expected = DoubleCalculator.Multiply(left, right);

            // Act
            var actual = subject.Multiply(eLeft, eRight).ToDouble();

            // Assert
            actual.Should().Be(expected);
        }

        [TestCase(2.3, 4.1, TestName = "testcase 01")]
        [TestCase(2.3e-20, -4.1, TestName = "testcase 02")]
        [TestCase(0.111112, 1.0e-12, TestName = "testcase 03")]
        public void Given_pair_of_doubles_When_Divide_Then_result_is_the_same_as_with_DoubleCalculator(
            double left,
            double right)
        {
            // Arrange
            var subject = new EFloatCalculator(EContext.Binary64, EFloat.Create(2, -32));
            var eLeft = EFloat.FromDouble(left);
            var eRight = EFloat.FromDouble(right);
            var expected = DoubleCalculator.Divide(left, right);

            // Act
            var actual = subject.Divide(eLeft, eRight).ToDouble();

            // Assert
            actual.Should().Be(expected);
        }

        [TestCase(2.3, TestName = "testcase 01")]
        [TestCase(-4.1, TestName = "testcase 02")]
        [TestCase(1.0e-12, TestName = "testcase 03")]
        public void Given_a_double_When_compute_Opposite_Then_result_is_the_same_as_with_DoubleCalculator(double number)
        {
            // Arrange
            var subject = new EFloatCalculator(EContext.Binary64, EFloat.Create(2, -32));
            var eNumber = EFloat.FromDouble(number);
            var expected = DoubleCalculator.Opposite(number);

            // Act
            var actual = subject.Opposite(eNumber).ToDouble();

            // Assert
            actual.Should().Be(expected);
        }

        [TestCase(2.3, TestName = "testcase 01")]
        [TestCase(4.1, TestName = "testcase 02")]
        [TestCase(1.0e-12, TestName = "testcase 03")]
        public void Given_a_double_When_compute_SquareRoot_Then_result_is_the_same_as_with_DoubleCalculator(
            double number)
        {
            // Arrange
            var subject = new EFloatCalculator(EContext.Binary64, EFloat.Create(2, -32));
            var eNumber = EFloat.FromDouble(number);
            var expected = DoubleCalculator.SquareRoot(number);

            // Act
            var actual = subject.SquareRoot(eNumber);

            // Assert
            actual.Should().Be(expected);
        }

        [TestCase(2.3, TestName = "testcase 01")]
        [TestCase(-4.1, TestName = "testcase 02")]
        [TestCase(1.0e-12, TestName = "testcase 03")]
        public void Given_a_double_When_compute_Sign_Then_result_is_the_same_as_with_DoubleCalculator(double number)
        {
            // Arrange
            var subject = new EFloatCalculator(EContext.Binary64, EFloat.Create(2, -32));
            var eNumber = EFloat.FromDouble(number);
            var expected = DoubleCalculator.Sign(number);

            // Act
            var actual = subject.Sign(eNumber);

            // Assert
            actual.Should().Be(expected);
        }
    }
}
