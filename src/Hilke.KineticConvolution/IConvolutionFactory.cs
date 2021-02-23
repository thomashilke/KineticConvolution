using Fractions;

namespace Hilke.KineticConvolution
{
    public interface IConvolutionFactory<TAlgebraicNumber>
    {
        IAlgebraicNumberCalculator<TAlgebraicNumber> AlgebraicNumberCalculator { get; }

        TAlgebraicNumber Zero { get; }

        TAlgebraicNumber One { get; }

        Point<TAlgebraicNumber> CreatePoint(TAlgebraicNumber x, TAlgebraicNumber y);

        Direction<TAlgebraicNumber> CreateDirection(TAlgebraicNumber x, TAlgebraicNumber y);

        DirectionRange<TAlgebraicNumber> CreateDirectionRange(
            Direction<TAlgebraicNumber> start,
            Direction<TAlgebraicNumber> end,
            Orientation orientation);

        Segment<TAlgebraicNumber> CreateSegment(
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Fraction weight);

        Segment<TAlgebraicNumber> CreateSegment(
            TAlgebraicNumber startX,
            TAlgebraicNumber startY,
            TAlgebraicNumber endX,
            TAlgebraicNumber endY,
            Fraction weight);

        Arc<TAlgebraicNumber> CreateArc(
            Point<TAlgebraicNumber> center,
            DirectionRange<TAlgebraicNumber> directions,
            TAlgebraicNumber radius,
            Fraction weight);

        Arc<TAlgebraicNumber> CreateArc(
            TAlgebraicNumber centerX,
            TAlgebraicNumber centerY,
            TAlgebraicNumber directionStartX,
            TAlgebraicNumber directionStartY,
            TAlgebraicNumber directionEndX,
            TAlgebraicNumber directionEndY,
            Orientation orientation,
            TAlgebraicNumber radius,
            Fraction weight);

        Convolution<TAlgebraicNumber> ConvolveShapes(
            Shape<TAlgebraicNumber> shape1,
            Shape<TAlgebraicNumber> shape2);
    }
}
