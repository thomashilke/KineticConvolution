using System;

using FluentAssertions;

using Hilke.KineticConvolution.DoubleAlgebraicNumber;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

using Hilke.KineticConvolution.Tests.TestCaseDataSource;

namespace Hilke.KineticConvolution.Tests
{
    [TestFixture]
    public class DirectionTests
    {
        private static readonly ConvolutionFactory Factory = new();
        private Direction<double> _subject;

        [OneTimeSetUp]
        public void OneTimeSetUp() =>
            _subject = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 100.5, y: 50.0);

        [Test]
        public void When_calling_Direction_With_null_calculator_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _ = new Direction<double>(null!, x: -2.1, y: 5.7);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should().Be(expected: "calculator");
        }

        [Test]
        public void When_calling_Direction_With_x_and_y_components_equal_to_zero_Then_an_ArgumentException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _ = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 0.0, y: 0.0);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentException>()
                  .WithMessage(expectedWildcardPattern: "Both components of the direction cannot be simultaneously zero. (Parameter 'y')");
        }

        [Test]
        public void When_calling_X_then_the_correct_value_Should_be_returned()
        {
            // Arrange
            var actual = _subject.X;
            const double expected = 100.5;

            // Assert
            actual.Should().Be(expected);
        }

        [Test]
        public void When_calling_Y_then_the_correct_value_Should_be_returned()
        {
            // Arrange
            var actual = _subject.Y;
            const double expected = 50.0;

            // Assert
            actual.Should().Be(expected);
        }

        [Test]
        public void When_calling_Determinant_With_null_parameter_Then_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _subject.Determinant(null!);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should().Be(expected: "other");
        }

        [TestCaseSource(
            typeof(DirectionTestCaseDataSource),
            nameof(DirectionTestCaseDataSource.TestCases))]
        public void When_calling_LastOf_With_valid_argument_The_result_Should_be_correct(
            Direction<double> subject,
            Direction<double> direction1,
            Direction<double> direction2,
            Direction<double> expected)
        {
            // Act
            var actual = subject.LastOf(direction1, direction2);

            // Assert
            actual.Should().Be(expected);
        }

        [Test]
        public void When_calling_StrictlyBelongsTo_with_null_directions_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _subject.StrictlyBelongsTo(null!);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be(expected: "directions");
        }

        [Test]
        public void When_calling_BelongsTo_with_null_directions_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            Action action = () => _subject.BelongsTo(null!);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be(expected: "directions");
        }

        [Test]
        public void When_calling_CompareTo_with_null_direction_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            var reference = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 0.5, y: 0.5);
            Action action = () => _subject.CompareTo(null!, reference);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be(expected: "direction");
        }

        [Test]
        public void When_calling_CompareTo_with_null_referenceDirection_Then_an_ArgumentNullException_Should_be_thrown()
        {
            // Arrange
            var direction = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 1.0, y: 0.0);
            Action action = () => _subject.CompareTo(direction, null!);

            // Assert
            action.Should()
                  .ThrowExactly<ArgumentNullException>()
                  .And.ParamName.Should()
                  .Be(expected: "referenceDirection");
        }

        [Test]
        public void When_calling_Equals_with_null_parameter_Then_the_result_Should_be_false()
        {
            // Arrange
            object other = null!;

            // Act
            var actual = _subject.Equals(other);

            // Assert
            actual.Should().BeFalse();
        }

        [Test]
        public void When_calling_Equals_with_different_types_Then_the_result_Should_be_false()
        {
            // Arrange
            object other = "John";

            // Act
            var actual = _subject.Equals(other);

            // Assert
            actual.Should().BeFalse();
        }

        [Test]
        public void When_calling_GetHashCode_on_distinct_object_with_same_values_Then_the_hashes_Should_be_equal()
        {
            // Arrange
            var direction1 = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 33.5, y: 12.738);
            var direction2 = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 33.5, y: 12.738);

            // Act
            var actual = direction1.GetHashCode();
            var expected = direction2.GetHashCode();

            // Assert
            direction1.Should().BeEquivalentTo(direction2);
            actual.Should().Be(expected);
        }

        [Test]
        public void When_calling_GetHashCode_on_distinct_object_with_different_values_Then_the_hashes_Should_be_different()
        {
            // Arrange
            var direction1 = new Direction<double>(Factory.AlgebraicNumberCalculator, x: 33.5, y: 3.5);
            var direction2 = new Direction<double>(Factory.AlgebraicNumberCalculator, x: -3.5, y: 12.738);

            // Act
            var actual = direction1.GetHashCode();
            var expected = direction2.GetHashCode();

            // Assert
            direction1.Should().NotBeEquivalentTo(direction2);
            actual.Should().NotBe(expected);
        }

        [Test]
        public void When_calling_Scale_Then_The_correct_result_Should_be_returned()
        {
            // Arrange
            const double scaleFactor = 2.0;
            var expected = new Direction<double>(Factory.AlgebraicNumberCalculator, scaleFactor * _subject.X, scaleFactor * _subject.Y);

            // Act
            var actual = _subject.Scale(scaleFactor);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void When_calling_not_equal_operator_on_same_instance_Then_the_result_Should_be_False()
        {
            // Arrange
            var other = _subject;

            // Act
            var actual = _subject != other;

            // Assert
            actual.Should().BeFalse();
        }

        [Test]
        public void When_Direction_Is_Given_Then_It_Should_Belongs_To_The_Expected_Half_Plan()
        {
            var east = Factory.CreateDirection(x: 3.0, y: 0.0);
            var west = Factory.CreateDirection(x: -2.0, y: 0.0);

            var lowerHalfPlan = Factory.CreateDirectionRange(east, west, Orientation.Clockwise);
            var upperHalfPlan = Factory.CreateDirectionRange(east, west, Orientation.CounterClockwise);

            var directionInUpperHalfPlan = Factory.CreateDirection(x: -5.0, y: 0.5);
            var directionInLowerHalfPlan = Factory.CreateDirection(x: -5.0, y: -0.5);

            directionInUpperHalfPlan.BelongsTo(upperHalfPlan).Should().BeTrue();

            directionInLowerHalfPlan.BelongsTo(lowerHalfPlan).Should().BeTrue();
            directionInLowerHalfPlan.BelongsTo(upperHalfPlan).Should().BeFalse();
        }

        [Test]
        public void When_Direction_Is_One_DirectionRange_Extremity_Then_Direction_Should_Not_Belongs_Strictly_To_DirectionRange()
        {
            var east = Factory.CreateDirection(x: 1.0, y: 0.0);
            var north = Factory.CreateDirection(x: 0.0, y: 1.0);

            var directionRange1 = Factory.CreateDirectionRange(east, north, Orientation.CounterClockwise);
            var directionRange2 = Factory.CreateDirectionRange(east, north, Orientation.Clockwise);

            east.BelongsTo(directionRange1).Should().BeTrue();
            east.BelongsTo(directionRange2).Should().BeTrue();
            north.BelongsTo(directionRange1).Should().BeTrue();
            north.BelongsTo(directionRange2).Should().BeTrue();

            east.StrictlyBelongsTo(directionRange1).Should().BeFalse();
            east.StrictlyBelongsTo(directionRange2).Should().BeFalse();
            north.StrictlyBelongsTo(directionRange1).Should().BeFalse();
            north.StrictlyBelongsTo(directionRange2).Should().BeFalse();
        }

        [Test]
        public void When_DirectionRange_Is_The_Complete_Circle_Then_All_Directions_Should_Belong_To_It()
        {
            // Arrange
            var east = Factory.CreateDirection(x: 5.0, y: 0.0);
            var west = Factory.CreateDirection(x: -5.0, y: 0.0);

            var clockwiseRange = Factory.CreateDirectionRange
                (east, east, Orientation.Clockwise);

            var counterClockwiseRange = Factory.CreateDirectionRange(
                east, east, Orientation.CounterClockwise);

            // Act
            var westBelongsToClockwiseRange = west.BelongsTo(clockwiseRange);
            var westBelongsToCounterClockwiseRange = west.BelongsTo(counterClockwiseRange);
            var eastBelongsToClockwiseRange = east.BelongsTo(clockwiseRange);
            var eastBelongsToCounterClockwiseRange = east.BelongsTo(counterClockwiseRange);

            // Assert
            westBelongsToClockwiseRange.Should().BeTrue();
            westBelongsToCounterClockwiseRange.Should().BeTrue();
            westBelongsToClockwiseRange.Should().BeTrue();
            westBelongsToCounterClockwiseRange.Should().BeTrue();
        }

        private static IEnumerable<TestCaseData> DirectionEqualityTestCaseSource()
        {
            yield return new TestCaseData(
                Factory.CreateDirection(x: 1.0, y: 2.0),
                Factory.CreateDirection(x: 2.0, y: 4.0),
                arg3: true);

            const double equalityTolerance = 1.0e-9;

            var north = Factory.CreateDirection(x: 0.0, y: 1.0);
            var east = Factory.CreateDirection(x: 5.0, y: 0.0);
            var south = Factory.CreateDirection(x: 0.0, y: -1.0);
            var west = Factory.CreateDirection(x: -5.0, y: 0.0);

            var cardinalDirections = new [] {north, east, south, west};

            foreach (var cardinalDirection in cardinalDirections)
            {
                yield return new TestCaseData(cardinalDirection, cardinalDirection, arg3: true);
            }

            foreach (var directions in cardinalDirections
                .SelectMany(direction =>
                    perturb(direction, 0.1 * equalityTolerance)
                    .Select(perturbedDirection => (direction, perturbedDirection))))
            {
                yield return new TestCaseData(directions.direction, directions.perturbedDirection, arg3: true);
            }

            foreach (var directions in cardinalDirections
                .SelectMany(direction =>
                    perturb(direction, 10000.0 * equalityTolerance)
                    .Select(perturbedDirection => (direction, perturbedDirection))))
            {
                yield return new TestCaseData(directions.direction, directions.perturbedDirection, arg3: false);
            }

            yield return new TestCaseData(north, south, arg3: false);

            yield return new TestCaseData(west, east, arg3: false);

            IEnumerable<Direction<double>> perturb(Direction<double> direction, double tolerance)
            {
                yield return Factory.CreateDirection(direction.X + tolerance, direction.Y + tolerance);
                yield return Factory.CreateDirection(direction.X - tolerance, direction.Y - tolerance);
                yield return Factory.CreateDirection(direction.X + tolerance, direction.Y - tolerance);
                yield return Factory.CreateDirection(direction.X - tolerance, direction.Y + tolerance);
            }
        }

        [TestCaseSource(nameof(DirectionEqualityTestCaseSource))]
        public void When_Directions_Are_Given_Then_Directions_Equality_Should_Be_As_Expected(
            Direction<double> direction1,
            Direction<double> direction2,
            bool expectedAreEquals) =>
        direction1.Equals(direction2).Should().Be(expectedAreEquals);

        private static IEnumerable<TestCaseData> _directionComparisonTestCaseSource()
        {
            var reference = Factory.CreateDirection(x: 1.0, y: 0.0);

            var direction1 = Factory.CreateDirection(x: -1.0, y: 0.0);
            var direction2 = Factory.CreateDirection(x: 1.0, y: -5.0);
            var direction3 = Factory.CreateDirection(x: 0.0, y: 1.0);
            var direction4 = Factory.CreateDirection(x: -1.0, y: 1.0);

            yield return new TestCaseData(direction1, direction2, reference, DirectionOrder.Before);
            yield return new TestCaseData(direction3, direction4, reference, DirectionOrder.Before);
            yield return new TestCaseData(direction2, direction1, reference, DirectionOrder.After);
            yield return new TestCaseData(direction4, direction3, reference, DirectionOrder.After);
            yield return new TestCaseData(direction2, direction2, reference, DirectionOrder.Equal);
            yield return new TestCaseData(direction1, direction2, direction2, DirectionOrder.After);
            yield return new TestCaseData(direction1, direction2, direction1, DirectionOrder.Before);
        }

        [TestCaseSource(nameof(_directionComparisonTestCaseSource))]
        public void When_Directions_Are_Given_Then_CompareTo_Should_Return_Expected_Result(
            Direction<double> direction1,
            Direction<double> direction2,
            Direction<double> reference,
            DirectionOrder expectedResult) =>
            direction1.CompareTo(direction2, reference).Should().Be(expectedResult);
    }
}
