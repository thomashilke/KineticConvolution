using System;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;
using Hilke.KineticConvolution.Helpers;

using NUnit.Framework;

namespace Hilke.KineticConvolution.Tests.Helpers
{
    [TestFixture(TestOf = typeof(VectorCalculator<>))]
    internal class VectorCalculatorTests
    {
        private static readonly DoubleAlgebraicNumberCalculator DoubleCalculator = new();

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_does_not_throw_exception()
        {
            // Arrange
            Action action = () => _ = new VectorCalculator<double>(DoubleCalculator);

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Given_valid_parameters_When_calling_constructor_Then_create_a_valid_instance()
        {
            // Act
            var subject = new VectorCalculator<double>(DoubleCalculator);

            // Assert
            subject.Should().NotBeNull().And.BeAssignableTo<VectorCalculator<double>>();
        }

        [Test]
        public void Given_invalid_parameters_When_calling_constructor_Then_throw_exception()
        {
            // Arrange
            Action action = () => _ = new VectorCalculator<double>(calculator: null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("calculator");
        }

        [Test]
        public void Given_invalid_arguments_When_calculate_Then_throw()
        {
            // Arrange
            var subject = new VectorCalculator<double>(DoubleCalculator);
            var validVector = new Vector<double>(1.0, 2.0);

            Action action0 = () => subject.FromDirection(direction: null);
            Action action00 = () => subject.FromPoint(point: null);

            Action action1 = () => subject.Add(left: null, validVector);
            Action action2 = () => subject.Add(validVector, right: null);

            Action action3 = () => subject.Subtract(left: null, validVector);
            Action action4 = () => subject.Subtract(validVector, right: null);

            Action action5 = () => subject.GetDot(left: null, validVector);
            Action action6 = () => subject.GetDot(validVector, right: null);

            Action action7 = () => subject.AreAlmostEqual(left: null, validVector, tolerance: 0.1);
            Action action8 = () => subject.AreAlmostEqual(validVector, right: null, tolerance: 0.1);

            Action action9 = () => subject.GetLength(vector: null);
            Action action10 = () => subject.GetOpposite(vector: null);
            Action action11 = () => subject.RotateThreeQuarterOfATurn(vector: null);

            Action action12 = () => subject.Multiply(1.2, vector: null);

            // Assert
            action0.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("direction");
            action00.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("point");

            action1.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("left");
            action2.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("right");

            action3.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("left");
            action4.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("right");

            action5.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("left");
            action6.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("right");

            action7.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("left");
            action8.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("right");

            action9.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("vector");
            action10.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("vector");
            action11.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("vector");

            action12.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("vector");
        }



        [Test]
        public void Given_valid_arguments_When_calculate_on_one_vector_Then_returns_expected()
        {
            // Arrange
            var subject = new VectorCalculator<double>(DoubleCalculator);
            var vector = new Vector<double>(2.0, 1.0);
            var factor = 2.1;

            var expectedLength = Math.Sqrt(2.0 * 2.0  + 1.0 * 1.0);
            var expectedRotated = new Vector<double>(1.0, -2.0);
            var expectedMultiplication = new Vector<double>(4.2, 2.1);

            // Act
            var actualLength = subject.GetLength(vector);
            var actualRotated = subject.RotateThreeQuarterOfATurn(vector);
            var actualMultiplication = subject.Multiply(factor, vector);

            // Assert
            actualLength.Should().Be(expectedLength);
            actualRotated.Should().BeEquivalentTo(expectedRotated);
            actualMultiplication.Should().BeEquivalentTo(expectedMultiplication);
        }

        [Test]
        public void Given_valid_arguments_When_calculate_on_pair_of_vectors_Then_returns_expected()
        {
            // Arrange
            var subject = new VectorCalculator<double>(DoubleCalculator);
            var vector1 = new Vector<double>(2.0, 1.0);
            var vector2 = new Vector<double>(1.0, 0.0);

            var expectedSum = new Vector<double>(3.0, 1.0);
            var expectedDifference = new Vector<double>(1.0, 1.0);
            var expectedDot = 2.0;

            // Act
            var actualSum = subject.Add(vector1, vector2);
            var actualDifference = subject.Subtract(vector1, vector2);
            var actualDot = subject.GetDot(vector1, vector2);

            // Assert
            actualSum.Should().BeEquivalentTo(expectedSum);
            actualDifference.Should().BeEquivalentTo(expectedDifference);
            actualDot.Should().Be(expectedDot);
        }


        [Test]
        public void Given_test_cases_When_query_quality_Then_returns_expected()
        {
            // Arrange
            var subject = new VectorCalculator<double>(DoubleCalculator);
            var vector = new Vector<double>(2.0, 1.0);
            var almostVector = new Vector<double>(2.0, 1.1);
            var tolerance = 0.1;

            // Act - Assert
            subject.AreAlmostEqual(vector, almostVector, tolerance).Should().BeTrue();
            subject.AreOrthogonal(vector, subject.RotateThreeQuarterOfATurn(vector)).Should().BeTrue();
        }
    }
}


