using System;
using System.Diagnostics;

namespace Hilke.KineticConvolution
{
    [DebuggerDisplay("({X}, {Y})")]
    public sealed class Point<TAlgebraicNumber> : IEquatable<Point<TAlgebraicNumber>>
    {
        private readonly IAlgebraicNumberCalculator<TAlgebraicNumber> _calculator;

        internal Point(IAlgebraicNumberCalculator<TAlgebraicNumber> calculator, TAlgebraicNumber x, TAlgebraicNumber y)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
        }

        public TAlgebraicNumber X { get; }

        public TAlgebraicNumber Y { get; }

        /// <inheritdoc />
        public bool Equals(Point<TAlgebraicNumber>? other)
        {
            return Equals(other as object);
        }

        public Point<TAlgebraicNumber> Translate(Direction<TAlgebraicNumber> direction, TAlgebraicNumber length)
        {
            var normalizedDirection = direction.Normalize();

            return new Point<TAlgebraicNumber>(
                _calculator,
                _calculator.Add(X, _calculator.Multiply(normalizedDirection.X, length)),
                _calculator.Add(Y, _calculator.Multiply(normalizedDirection.Y, length)));
        }

        public Direction<TAlgebraicNumber> DirectionTo(Point<TAlgebraicNumber> target) =>
            new Direction<TAlgebraicNumber>(
                _calculator,
                _calculator.Subtract(target.X, X),
                _calculator.Subtract(target.Y, Y));

        public Point<TAlgebraicNumber> Sum(Point<TAlgebraicNumber> point2) =>
            new Point<TAlgebraicNumber>(_calculator, _calculator.Add(X, point2.X), _calculator.Add(Y, point2.Y));

        /// <inheritdoc />
        public override bool Equals(object? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is Point<TAlgebraicNumber> point)
            {
                return _calculator.AreEqual(X, point.X)
                    && _calculator.AreEqual(Y, point.Y);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X!.GetHashCode() * 397) ^ Y!.GetHashCode();
            }
        }

        public static bool operator ==(Point<TAlgebraicNumber>? left, Point<TAlgebraicNumber>? right) =>
            Equals(left, right);

        public static bool operator !=(Point<TAlgebraicNumber>? left, Point<TAlgebraicNumber>? right) =>
            !Equals(left, right);
    }
}
