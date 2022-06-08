using System;
using System.Linq;

namespace Hilke.KineticConvolution.Helpers
{
    public class VectorCalculator<TAlgebraicNumber>
    {
        private readonly IAlgebraicNumberCalculator<TAlgebraicNumber> _calculator;
        private readonly ConvolutionFactory<TAlgebraicNumber> _factory;

        public VectorCalculator(IAlgebraicNumberCalculator<TAlgebraicNumber> calculator)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _factory = new ConvolutionFactory<TAlgebraicNumber>(_calculator);
        }

        public TAlgebraicNumber GetDot(Vector<TAlgebraicNumber> left, Vector<TAlgebraicNumber> right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.Dimension != right.Dimension)
            {
                throw new ArgumentException("The vectors must have the same dimension.");
            }

            return left.Coordinates.Zip(
                           right.Coordinates,
                           (leftCoordinate, rightCoordinate) => _calculator.Multiply(leftCoordinate, rightCoordinate))
                       .Aggregate((sum, next) => _calculator.Add(sum, next));
        }

        public TAlgebraicNumber GetLength(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            return _calculator.SquareRoot(GetDot(vector, vector));
        }

        public bool AreAlmostEqual(
            Vector<TAlgebraicNumber> left,
            Vector<TAlgebraicNumber> right,
            TAlgebraicNumber tolerance)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.Dimension != right.Dimension)
            {
                throw new ArgumentException("The vectors must have the same dimension.");
            }

            if (tolerance is null)
            {
                throw new ArgumentNullException(nameof(tolerance));
            }

            return _calculator.IsSmallerThan(GetLength(Subtract(left, right)), tolerance);
        }

        public bool AreOrthogonal(
            Vector<TAlgebraicNumber> left,
            Vector<TAlgebraicNumber> right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.Dimension != right.Dimension)
            {
                throw new ArgumentException("The vectors must have the same dimension.");
            }

            return _calculator.IsZero(GetDot(left, right));
        }

        public Vector<TAlgebraicNumber> Add(Vector<TAlgebraicNumber> left, Vector<TAlgebraicNumber> right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.Dimension != right.Dimension)
            {
                throw new ArgumentException("The vectors must have the same dimension.");
            }

            return Vector<TAlgebraicNumber>.FromEnumerable(
                left.Coordinates.Zip(
                    right.Coordinates,
                    (leftCoordinate, rightCoordinate) =>
                        _calculator.Add(
                            leftCoordinate,
                            rightCoordinate)));
        }

        public Vector<TAlgebraicNumber> GetOpposite(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            return Vector<TAlgebraicNumber>.FromEnumerable(
                vector.Coordinates.Select(
                    coordinate =>
                        _calculator.Opposite(coordinate)));
        }

        public Vector<TAlgebraicNumber> Subtract(Vector<TAlgebraicNumber> left, Vector<TAlgebraicNumber> right)
        {
            if (left is null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.Dimension != right.Dimension)
            {
                throw new ArgumentException("The vectors must have the same dimension.");
            }

            return Add(left, GetOpposite(right));
        }

        public Vector<TAlgebraicNumber> Multiply(TAlgebraicNumber factor, Vector<TAlgebraicNumber> vector)
        {
            if (factor is null)
            {
                throw new ArgumentNullException(nameof(factor));
            }

            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            return Vector<TAlgebraicNumber>.FromEnumerable(
                vector.Coordinates.Select(
                    coordinate =>
                        _calculator.Multiply(factor, coordinate)));
        }

        public Vector<TAlgebraicNumber> FromPoint(Point<TAlgebraicNumber> point)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            return Vector<TAlgebraicNumber>.FromEnumerable(new[] { point.X, point.Y });
        }

        public Vector<TAlgebraicNumber> FromDirection(Direction<TAlgebraicNumber> direction)
        {
            if (direction is null)
            {
                throw new ArgumentNullException(nameof(direction));
            }

            return Vector<TAlgebraicNumber>.FromEnumerable(new[] { direction.X, direction.Y });
        }

        public Point<TAlgebraicNumber> ToPoint(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            if (vector.Coordinates.Count != 2)
            {
                throw new ArgumentException(
                    $"The vector must have exactly 2 coordinates. Found {vector.Coordinates.Count}.",
                    nameof(vector));
            }

            return _factory.CreatePoint(vector.Coordinates[0], vector.Coordinates[1]);
        }

        public Direction<TAlgebraicNumber> ToDirection(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            if (vector.Coordinates.Count != 2)
            {
                throw new ArgumentException(
                    $"The vector must have exactly 2 coordinates. Found {vector.Coordinates.Count}.",
                    nameof(vector));
            }

            return _factory.CreateDirection(vector.Coordinates[0], vector.Coordinates[1]);
        }

        public Vector<TAlgebraicNumber> RotateThreeQuarterOfATurn(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            if (vector.Coordinates.Count != 2)
            {
                throw new ArgumentException(
                    $"The vector must have exactly 2 coordinates. Found {vector.Coordinates.Count}.",
                    nameof(vector));
            }

            return Vector<TAlgebraicNumber>.FromEnumerable(
                new[] { vector.Coordinates[1], _calculator.Opposite(vector.Coordinates[0]) });
        }
    }
}
