using Fractions;

namespace Hilke.KineticConvolution.Double
{
    public class Segment<TAlgebraicNumber> : Tracing<TAlgebraicNumber>
        where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        internal Segment(
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Direction<TAlgebraicNumber> startDirection,
            Direction<TAlgebraicNumber> endDirection,
            Fraction weight)
            : base(start, end, startDirection, endDirection, weight) { }

        public Direction<TAlgebraicNumber> Direction() =>
            new Direction<TAlgebraicNumber>(
                End.X.Subtract(Start.X),
                End.Y.Subtract(Start.Y));

        public Direction<TAlgebraicNumber> NormalDirection()
        {
            var direction = Direction();
            return new Direction<TAlgebraicNumber>(direction.Y.Opposite(), direction.X);
        }
    }
}
