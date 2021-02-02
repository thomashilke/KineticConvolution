using System;

using Fractions;

namespace Hilke.KineticConvolution
{
    public interface IConvolutionFactory<TAlgebraicNumber>
    {
        IAlgebraicNumberCalculator<TAlgebraicNumber> AlgebraicNumberCalculator { get; }

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

        public Segment<TAlgebraicNumber> CreateSegment(
            Fraction weight,
            TAlgebraicNumber startX,
            TAlgebraicNumber startY,
            TAlgebraicNumber endX,
            TAlgebraicNumber endY);

        Arc<TAlgebraicNumber> CreateArc(
            Fraction weight,
            Point<TAlgebraicNumber> center,
            DirectionRange<TAlgebraicNumber> directions,
            TAlgebraicNumber radius);

        Arc<TAlgebraicNumber> CreateArc(
            Fraction weight,
            TAlgebraicNumber centerX,
            TAlgebraicNumber centerY,
            TAlgebraicNumber directionStartX,
            TAlgebraicNumber directionStartY,
            TAlgebraicNumber directionEndX,
            TAlgebraicNumber directionEndY,
            Orientation orientation,
            TAlgebraicNumber radius);

        Convolution<TAlgebraicNumber> ConvolveShapes(
            Shape<TAlgebraicNumber> shape1,
            Shape<TAlgebraicNumber> shape2);
    }
}
