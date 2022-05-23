using System;

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

        public Vector<TAlgebraicNumber> FromPoint(Point<TAlgebraicNumber> point)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            return new Vector<TAlgebraicNumber>(point.X, point.Y);
        }

        public Vector<TAlgebraicNumber> FromDirection(Direction<TAlgebraicNumber> direction)
        {
            if (direction is null)
            {
                throw new ArgumentNullException(nameof(direction));
            }

            return new Vector<TAlgebraicNumber>(direction.X, direction.Y);
        }

        public Point<TAlgebraicNumber> ToPoint(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            return _factory.CreatePoint(vector.X, vector.Y);
        }

        public Direction<TAlgebraicNumber> ToDirection(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            return _factory.CreateDirection(vector.X, vector.Y);
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

            return _calculator.Add(
                _calculator.Multiply(left.X, right.X),
                _calculator.Multiply(left.Y, right.Y));
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

            if (tolerance is null)
            {
                throw new ArgumentNullException(nameof(tolerance));
            }

            return _calculator.IsSmallerThan(GetLength(Subtract(left, right)), tolerance);
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

            return new Vector<TAlgebraicNumber>(
                _calculator.Add(left.X, right.X),
                _calculator.Add(left.Y, right.Y));
        }

        public Vector<TAlgebraicNumber> GetOpposite(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            return new Vector<TAlgebraicNumber>(_calculator.Opposite(vector.X), _calculator.Opposite(vector.Y));
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

            return new Vector<TAlgebraicNumber>(
                _calculator.Multiply(factor, vector.X),
                _calculator.Multiply(factor, vector.Y));
        }

        public Vector<TAlgebraicNumber> RotateThreeQuarterOfATurn(Vector<TAlgebraicNumber> vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector));
            }

            return new Vector<TAlgebraicNumber>(vector.Y, _calculator.Opposite(vector.X));
        }
    }
}
