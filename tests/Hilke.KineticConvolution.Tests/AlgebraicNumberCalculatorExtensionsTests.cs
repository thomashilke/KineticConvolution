using System.Linq;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class AlgebraicNumberCalculatorTests
    {
        [TestCase(0.0, 0.0)]
        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 0.0)]
        [TestCase(-3.2, 1.1)]
        public void When_Number1_Is_Smaller_Than_Number2_Then_IsSmallerThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison =
                calculator.IsSmallerThan(number1, number2);

            // assert
            comparison.Should().BeTrue();
        }

        [TestCase(1.0, 0.0)]
        [TestCase(1.0, -2.0)]
        public void When_Number1_Is_Strictly_Greater_Than_Number2_Then_IsSmallerThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison = calculator.IsSmallerThan(number1, number2);

            // assert
            comparison.Should().BeFalse();
        }

        [TestCase(0.0, 0.0)]
        [TestCase(1.0, 0.0)]
        [TestCase(0.0, -1.0)]
        [TestCase(1.1, -3.2)]
        public void When_Number1_Is_Greater_Than_Number2_Then_IsGreaterThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison =
                calculator.IsGreaterThan(number1, number2);

            // assert
            comparison.Should().BeTrue();
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-2.0, 1.0)]
        public void When_Number1_Is_Strictly_Smaller_Than_Number2_Then_IsGreaterThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison = calculator.IsGreaterThan(number1, number2);

            // assert
            comparison.Should().BeFalse();
        }

        [TestCase(1.0, 0.0)]
        [TestCase(0.0, -1.0)]
        [TestCase(1.1, -3.2)]
        public void When_Number1_Is_Strictly_Greater_Than_Number2_Then_IsStrictlyGreaterThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison =
                calculator.IsStrictlyGreaterThan(number1, number2);

            // assert
            comparison.Should().BeTrue();
        }

        [TestCase(0.0, 0.0)]
        [TestCase(0.0, 1.0)]
        [TestCase(-2.0, 1.0)]
        public void When_Number1_Is_Smaller_Than_Number2_Then_IsStrictlyGreaterThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison = calculator.IsStrictlyGreaterThan(number1, number2);

            // assert
            comparison.Should().BeFalse();
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 0.0)]
        [TestCase(-3.2, 1.1)]
        public void When_Number1_Is_Strictly_Smaller_Than_Number2_Then_IsStrictlySmallerThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison =
                calculator.IsStrictlySmallerThan(number1, number2);

            // assert
            comparison.Should().BeTrue();
        }

        [TestCase(0.0, 0.0)]
        [TestCase(1.0, 0.0)]
        [TestCase(1.0, -2.0)]
        public void When_Number1_Is_Greater_Than_Number2_Then_IsStrictlySmallerThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var comparison = calculator.IsStrictlySmallerThan(number1, number2);

            // assert
            comparison.Should().BeFalse();
        }

        [TestCase(-1.0, 1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(-0.0, 0.0)]
        [TestCase(1.0, 1.0)]
        public void When_(double doubleNumber, double expectedAbsoluteValue)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number = calculator.CreateConstant(doubleNumber);

            // act
            var absoluteValue = calculator.Abs(number);

            // assert
            absoluteValue.Should().Be(expectedAbsoluteValue);
        }

        [TestCase(0.0, 0.0, true)]
        [TestCase(3.0, 0.0, false)]
        [TestCase(0.0, 4.0, false)]
        [TestCase(-2.0, -2.0, true)]
        [TestCase(2.0, 2.0, true)]
        public void When_Numbers_Are_Equal_Then_AreEqual_Should_Return_Expected_Result(
            double doubleNumber1, double doubleNumber2, bool number1EqualsNumber2Expectation)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);

            // act
            var number1EqualsNumber2 = calculator.AreEqual(number1, number2);

            // assert
            number1EqualsNumber2.Should().Be(number1EqualsNumber2Expectation);
        }

        [TestCase(0.0, 0.0, 0.0, true)]
        [TestCase(1.0, 1.05, 0.1, true)]
        [TestCase(1.0, 0.95, 0.1, true)]
        [TestCase(1.0, 1.15, 0.1, false)]
        [TestCase(1.0, 0.85, 0.1, false)]
        public void When_Numbers_Are_Close_Then_AreClose_Should_Return_True(
            double doubleNumber1,
            double doubleNumber2,
            double doubleTolerance,
            bool numberAreCloseExpectation)
        {
            // arrange
            var factory = new ConvolutionFactory();

            var calculator = factory.AlgebraicNumberCalculator;

            var number1 = calculator.CreateConstant(doubleNumber1);
            var number2 = calculator.CreateConstant(doubleNumber2);
            var tolerance = calculator.CreateConstant(doubleTolerance);

            // act
            var number1IsCloseToNumber2 = calculator.AreClose(number1, number2, tolerance);

            // assert
            number1IsCloseToNumber2.Should().Be(numberAreCloseExpectation);
        }
    }
}
