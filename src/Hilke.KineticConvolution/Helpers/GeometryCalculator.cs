﻿using System;


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
            var vc = _vectorCalculator;

            var tangentBefore = vc.FromDirection(tangentDirectionBefore.Normalize());
            var tangentAfter = vc.FromDirection(tangentDirectionAfter.Normalize());
            var rightNormalBefore = vc.RotateThreeQuarterOfATurn(tangentBefore);
            var rightNormalAfter = vc.RotateThreeQuarterOfATurn(tangentAfter);

            var dotProduct = vc.GetDot(tangentBefore, rightNormalAfter);
            if (_calculator.IsZero(dotProduct))
            {
                var haveSameDirections = _calculator.IsPositive(vc.GetDot(tangentBefore, tangentAfter));

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
                        start: vc.ToDirection(rightNormalBefore),
                        end: vc.ToDirection(rightNormalAfter),
                                            orientation: Orientation.CounterClockwise),
                    radius: _calculator.CreateConstant(0.0),
                    weight: 1);

            Arc<TAlgebraicNumber> createArcForNonAlignedPoints()
            {
                var IsAngleCounterClockwise = _calculator.IsPositive(dotProduct);

                var start = IsAngleCounterClockwise ? vc.ToDirection(rightNormalBefore) : vc.ToDirection(rightNormalBefore).Opposite();
                var end = IsAngleCounterClockwise ? vc.ToDirection(rightNormalAfter) : vc.ToDirection(rightNormalAfter).Opposite();
                var orientation = IsAngleCounterClockwise ? Orientation.CounterClockwise : Orientation.Clockwise;
                var directions = _convolutionFactory.CreateDirectionRange(start, end, orientation);

                var center = _calculator.IsZero(radius)
                                 ? cornerPoint
                                 : computeCenterForNonZeroRadius();

                return _convolutionFactory.CreateArc(center, directions, radius, weight: 1);

                Point<TAlgebraicNumber> computeCenterForNonZeroRadius()
                {
                    var oppositeTangentBefore = vc.GetOpposite(tangentBefore);
                    var bisectorDirection = vc.Add(oppositeTangentBefore, tangentAfter);
                    var parameterOnBisectorLine = _calculator.Divide(radius, _calculator.Abs(dotProduct));
                    var translationLength = _calculator.Multiply(parameterOnBisectorLine, vc.GetLength(bisectorDirection));

                    return cornerPoint.Translate(direction: vc.ToDirection(bisectorDirection), length: translationLength);
                }
            }
        }
    }
}