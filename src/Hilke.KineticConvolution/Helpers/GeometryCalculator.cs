using System;

namespace Hilke.KineticConvolution.Helpers
{
    public class GeometryCalculator<TAlgebraicNumber>
    {
        private readonly IAlgebraicNumberCalculator<TAlgebraicNumber> _calculator;
        private readonly ConvolutionFactory<TAlgebraicNumber> _convolutionFactory;
        private readonly VectorCalculator<TAlgebraicNumber> _vectorCalculator;

        public GeometryCalculator(IAlgebraicNumberCalculator<TAlgebraicNumber> calculator)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _convolutionFactory = new ConvolutionFactory<TAlgebraicNumber>(_calculator);
            _vectorCalculator = new VectorCalculator<TAlgebraicNumber>(_calculator);
        }

        public Tracing<TAlgebraicNumber>? CreateCornerArc(
            Direction<TAlgebraicNumber> tangentDirectionBefore,
            Point<TAlgebraicNumber> cornerPoint,
            Direction<TAlgebraicNumber> tangentDirectionAfter,
            TAlgebraicNumber radius)
        {
            var tangentBefore = _vectorCalculator.FromDirection(tangentDirectionBefore.Normalize());
            var tangentAfter = _vectorCalculator.FromDirection(tangentDirectionAfter.Normalize());
            var rightNormalBefore = _vectorCalculator.RotateThreeQuarterOfATurn(tangentBefore);
            var rightNormalAfter = _vectorCalculator.RotateThreeQuarterOfATurn(tangentAfter);

            var dotProduct = _vectorCalculator.GetDot(tangentBefore, rightNormalAfter);
            if (_calculator.IsZero(dotProduct))
            {
                var haveSameDirections = _calculator.IsPositive(_vectorCalculator.GetDot(tangentBefore, tangentAfter));

                if (!haveSameDirections && !_calculator.IsZero(radius))
                {
                    throw new ArgumentException(
                        "For an extremity arc (360°), the radius must be zero.",
                        nameof(radius));
                }

                return haveSameDirections ? null : createExtremityFullArc();
            }

            return createArcForNonAlignedPoints();

            Arc<TAlgebraicNumber> createExtremityFullArc() =>
                _convolutionFactory.CreateArc(
                    center: cornerPoint,
                    directions: _convolutionFactory.CreateDirectionRange(
                        start: _vectorCalculator.ToDirection(rightNormalBefore),
                        end: _vectorCalculator.ToDirection(rightNormalAfter),
                        orientation: Orientation.CounterClockwise),
                    radius: _calculator.FromDouble(0.0),
                    weight: 1);

            Arc<TAlgebraicNumber> createArcForNonAlignedPoints()
            {
                var IsAngleCounterClockwise = _calculator.IsPositive(dotProduct);

                var start = IsAngleCounterClockwise
                                ? _vectorCalculator.ToDirection(rightNormalBefore)
                                : _vectorCalculator.ToDirection(rightNormalBefore).Opposite();
                var end = IsAngleCounterClockwise
                              ? _vectorCalculator.ToDirection(rightNormalAfter)
                              : _vectorCalculator.ToDirection(rightNormalAfter).Opposite();
                var orientation = IsAngleCounterClockwise ? Orientation.CounterClockwise : Orientation.Clockwise;
                var directions = _convolutionFactory.CreateDirectionRange(start, end, orientation);

                var center = _calculator.IsZero(radius)
                                 ? cornerPoint
                                 : computeCenterForNonZeroRadius();

                return _convolutionFactory.CreateArc(center, directions, radius, weight: 1);

                Point<TAlgebraicNumber> computeCenterForNonZeroRadius()
                {
                    var oppositeTangentBefore = _vectorCalculator.GetOpposite(tangentBefore);
                    var bisectorDirection = _vectorCalculator.Add(oppositeTangentBefore, tangentAfter);
                    var parameterOnBisectorLine = _calculator.Divide(radius, _calculator.Abs(dotProduct));
                    var translationLength = _calculator.Multiply(
                        parameterOnBisectorLine,
                        _vectorCalculator.GetLength(bisectorDirection));

                    return cornerPoint.Translate(
                        direction: _vectorCalculator.ToDirection(bisectorDirection),
                        length: translationLength);
                }
            }
        }
    }
}
