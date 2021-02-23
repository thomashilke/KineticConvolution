using System;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class PointTests
    {
        private Point<double> _subject;
        private IAlgebraicNumberCalculator<double> _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new DoubleAlgebraicNumberCalculator();
            _subject = new Point<double>(_calculator, x: 3.5, y: 10.7);
        }

        [Test]
        public void Calling_Point_With_null_calculator_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _ = new Point<double>(null!, x: 3.5, y: 10.5);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be(expected: "calculator");
        }

        [Test]
        public void Calling_Equals_With_null_parameter_Then_the_result_Should_be_false()
        {
            // Arrange
            object other = null!;

            // Act
            var actual = _subject.Equals(other);

            // Assert
            actual.Should().BeFalse();
        }

        [Test]
        public void Calling_Equals_on_same_reference_Then_the_result_Should_be_true()
        {
            // Arrange
            object other = _subject;

            // Act
            var actual = _subject.Equals(other);

            // Assert
            actual.Should().BeTrue();
        }

        [Test]
        public void Calling_X_on_valid_point_Then_the_correct_value_Should_be_returned()
        {
            // Arrange
            const double expected = 3.5;

            // Act
            var actual = _subject.X;

            // Assert
            actual.Should().Be(expected);
        }

        [Test]
        public void Calling_Y_on_valid_point_Then_the_correct_value_Should_be_returned()
        {
            // Arrange
            const double expected = 10.7;

            // Act
            var actual = _subject.Y;

            // Assert
            actual.Should().Be(expected);
        }

        [Test]
        public void Calling_GetHashCode_on_two_distinct_instances_with_same_values_Should_return_the_same_hash()
        {
            // Arrange
            var other = new Point<double>(_calculator, _subject.X, _subject.Y);
            var expected = other.GetHashCode();

            // Act
            var actual = _subject.GetHashCode();

            // Assert
            actual.Should().Be(expected);
            _subject.Equals((object)other).Should().BeTrue();
        }

        [Test]
        public void Calling_Equals_on_two_distinct_instances_with_same_values_Should_return_the_same_true()
        {
            // Arrange
            var other = new Point<double>(_calculator, _subject.X, _subject.Y);

            // Act
            var actual = _subject.Equals(other);

            // Assert
            actual.Should().BeTrue();
        }

        [Test]
        public void
            Calling_GetHashCode_on_two_distinct_instances_with_different_values_Should_return_the_different_hash()
        {
            // Arrange
            var other = new Point<double>(_calculator, _subject.X, _subject.Y + 0.01);
            var expected = other.GetHashCode();

            // Act
            var actual = _subject.GetHashCode();

            // Assert
            actual.Should().NotBe(expected);
            _subject.Equals((object)other).Should().BeFalse();
        }

        [Test]
        public void Calling_not_equal_operator_on_two_distinct_instances_with_different_values_Should_return_true()
        {
            // Arrange
            var other = new Point<double>(_calculator, _subject.X, _subject.Y + 0.01);

            // Act
            var actual = _subject != other;

            // Assert
            actual.Should().BeTrue();
        }
    }
}
