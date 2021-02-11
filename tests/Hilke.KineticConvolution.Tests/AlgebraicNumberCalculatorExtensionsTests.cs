using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class AlgebraicNumberCalculatorTests
    {
        private IAlgebraicNumberCalculator<double> _calculator;

        [SetUp]
        public void SetUp()
        {
            var factory = new ConvolutionFactory();
            _calculator = factory.AlgebraicNumberCalculator;
        }

        [TestCase(0.0, 0.0)]
        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 0.0)]
        [TestCase(-3.2, 1.1)]
        public void When_Number1_Is_Smaller_Than_Number2_Then_IsSmallerThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsSmallerThan(number1, number2);

            // Assert
            comparison.Should().BeTrue();
        }

        [TestCase(1.0, 0.0)]
        [TestCase(1.0, -2.0)]
        public void When_Number1_Is_Strictly_Greater_Than_Number2_Then_IsSmallerThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsSmallerThan(number1, number2);

            // Assert
            comparison.Should().BeFalse();
        }

        [TestCase(0.0, 0.0)]
        [TestCase(1.0, 0.0)]
        [TestCase(0.0, -1.0)]
        [TestCase(1.1, -3.2)]
        public void When_Number1_Is_Greater_Than_Number2_Then_IsGreaterThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsGreaterThan(number1, number2);

            // Assert
            comparison.Should().BeTrue();
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-2.0, 1.0)]
        public void When_Number1_Is_Strictly_Smaller_Than_Number2_Then_IsGreaterThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsGreaterThan(number1, number2);

            // Assert
            comparison.Should().BeFalse();
        }

        [TestCase(1.0, 0.0)]
        [TestCase(0.0, -1.0)]
        [TestCase(1.1, -3.2)]
        public void When_Number1_Is_Strictly_Greater_Than_Number2_Then_IsStrictlyGreaterThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsStrictlyGreaterThan(number1, number2);

            // Assert
            comparison.Should().BeTrue();
        }

        [TestCase(0.0, 0.0)]
        [TestCase(0.0, 1.0)]
        [TestCase(-2.0, 1.0)]
        public void When_Number1_Is_Smaller_Than_Number2_Then_IsStrictlyGreaterThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsStrictlyGreaterThan(number1, number2);

            // Assert
            comparison.Should().BeFalse();
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 0.0)]
        [TestCase(-3.2, 1.1)]
        public void When_Number1_Is_Strictly_Smaller_Than_Number2_Then_IsStrictlySmallerThan_Should_Return_True(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsStrictlySmallerThan(number1, number2);

            // Assert
            comparison.Should().BeTrue();
        }

        [TestCase(0.0, 0.0)]
        [TestCase(1.0, 0.0)]
        [TestCase(1.0, -2.0)]
        public void When_Number1_Is_Greater_Than_Number2_Then_IsStrictlySmallerThan_Should_Return_False(
            double doubleNumber1, double doubleNumber2)
        {
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var comparison = _calculator.IsStrictlySmallerThan(number1, number2);

            // Assert
            comparison.Should().BeFalse();
        }

        [TestCase(-1.0, 1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(-0.0, 0.0)]
        [TestCase(1.0, 1.0)]
        public void Number_Absolute_Value_Should_Have_Expected_Value(double doubleNumber, double expectedAbsoluteValue)
        {
            // Arrange
            var number = _calculator.CreateConstant(doubleNumber);

            // Act
            var absoluteValue = _calculator.Abs(number);

            // Assert
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
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);

            // Act
            var number1EqualsNumber2 = _calculator.AreEqual(number1, number2);

            // Assert
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
            // Arrange
            var number1 = _calculator.CreateConstant(doubleNumber1);
            var number2 = _calculator.CreateConstant(doubleNumber2);
            var tolerance = _calculator.CreateConstant(doubleTolerance);

            // Act
            var number1IsCloseToNumber2 = _calculator.AreClose(number1, number2, tolerance);

            // Assert
            number1IsCloseToNumber2.Should().Be(numberAreCloseExpectation);
        }

        [TestCase(-1, 1, ExpectedResult = -1)]
        [TestCase(1, 1, ExpectedResult = 1)]
        [TestCase(1, -1, ExpectedResult = -1)]
        public double When_Calling_Min_Given_Parameter_Should_Return_Expected_Result(
            double number1, double number2) =>
            _calculator.Min(number1, number2);

        [TestCase(-1, 1, ExpectedResult = 1)]
        [TestCase(1, 1, ExpectedResult = 1)]
        [TestCase(1, -1, ExpectedResult = 1)]
        public double When_Calling_Max_Given_Parameter_Should_Return_Expected_Result(
            double number1, double number2) =>
            _calculator.Max(number1, number2);

        [TestCase(-1.0, 1.0)]
        [TestCase(2.0, 2.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(1.0, -10.0)]
        public void When_Calling_Sort_Given_Parameter_Should_Return_Expected_Result(
            double number1, double number2)
        {
            // Act
            var pair = _calculator.Sort(number1, number2);

            // Assert
            pair.Item1.Should().BeLessOrEqualTo(pair.Item2);
        }

        [TestCase(-2, -3, -1, ExpectedResult = true)]
        [TestCase(-2, -1, -3, ExpectedResult = false)]
        [TestCase(-1, -2, -3, ExpectedResult = false)]
        [TestCase(-1, -3, -2, ExpectedResult = false)]
        [TestCase(2, 3, 1, ExpectedResult = false)]
        [TestCase(2, 1, 3, ExpectedResult = true)]
        [TestCase(1, 2, 3, ExpectedResult = false)]
        [TestCase(1, 3, 2, ExpectedResult = false)]
        public bool When_Calling_IsInsideRange_Given_Parameter_Should_Return_Expected_Result(
            double number,
            double lowerBound,
            double higherBound) =>
            _calculator.IsInsideRange(number, lowerBound, higherBound);
    }
}
