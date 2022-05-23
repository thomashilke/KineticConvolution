using System;
using System.Collections.Generic;
using System.Linq;

using Fractions;

using MoreLinq;

namespace Hilke.KineticConvolution.Helpers
{
    internal class ShapeBuilder<TAlgebraicNumber>
    {
        private readonly ConvolutionFactory<TAlgebraicNumber> _convolutionFactory;
        private readonly IAlgebraicNumberCalculator<TAlgebraicNumber> _calculator;
        private readonly GeometryCalculator<TAlgebraicNumber> _geometryCalculator;
        private Point<TAlgebraicNumber>? _startPoint;
        private readonly List<(Point<TAlgebraicNumber> point, TAlgebraicNumber radius)> _corners;

        public ShapeBuilder(ConvolutionFactory<TAlgebraicNumber> factory)
        {
            _convolutionFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            _calculator = factory.AlgebraicNumberCalculator;
            _geometryCalculator = new GeometryCalculator<TAlgebraicNumber>(_calculator);

            _startPoint = null;
            _corners = new List<(Point<TAlgebraicNumber> point, TAlgebraicNumber radius)>();
        }

        public ShapeBuilder<TAlgebraicNumber> StartAt(Point<TAlgebraicNumber> start)
        {
            if (start is null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            _startPoint = start;

            return this;
        }

        public ShapeBuilder<TAlgebraicNumber> AddCorner(Point<TAlgebraicNumber> point, TAlgebraicNumber radius)
        {
            if (point is null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            if (_startPoint is null)
            {
                throw new InvalidOperationException("Start with a start point.");
            }

            _corners.Add((point, radius));

            return this;
        }

        public ShapeBuilder<TAlgebraicNumber> CloseWith(TAlgebraicNumber radius)
        {
            if (_startPoint is null)
            {
                throw new InvalidOperationException("Start with a start point.");
            }

            if (_corners.Count < 1)
            {
                throw new InvalidOperationException("At least one corner is needed.");
            }

            _corners.Add((_startPoint, radius));

            return this;
        }

        public Shape<TAlgebraicNumber> Build()
        {
            if (_corners.Count < 2)
            {
                throw new InvalidOperationException("At least two points are needed.");
            }

            var arcs = createCorners().ToList();
            var lastIndex = arcs.Count - 1;

            var tracings =
                new [] { arcs[lastIndex] }
                    .Concat(createSegmentIfNecessary(arcs[lastIndex].End, arcs[0].Start))
                    .Concat(
                        arcs.Pairwise(
                                (current, next) => new[] { current }
                                    .Concat(createSegmentIfNecessary(current.End, next.Start)))
                            .SelectMany(e => e))
                    .Where(tracing => tracing is not SinglePoint<TAlgebraicNumber>)
                    .ToList();

            return _convolutionFactory.CreateShape(tracings);

            IEnumerable<Tracing<TAlgebraicNumber>> createCorners()
            {
                for (var index = 0; index < _corners.Count; index++)
                {
                    var previousIndex = modulus(index - 1, _corners.Count);
                    var nextIndex = modulus(index + 1, _corners.Count);

                    yield return _geometryCalculator.CreateCornerArc(
                                     _corners[previousIndex].point.DirectionTo(_corners[index].point),
                                     _corners[index].point,
                                     _corners[index].point.DirectionTo(_corners[nextIndex].point),
                                     _corners[index].radius)
                              ?? new SinglePoint<TAlgebraicNumber>(
                                     _calculator,
                                     _corners[index].point,
                                     _corners[previousIndex].point.DirectionTo(_corners[index].point),
                                     1);
                }
            }

            int modulus(int dividend, int divisor)
            {
                var result = dividend % divisor;
                return result >= 0 ? result : result + divisor; 
            }

            IEnumerable<Segment<TAlgebraicNumber>> createSegmentIfNecessary(
                Point<TAlgebraicNumber> start,
                Point<TAlgebraicNumber> end) =>
                start != end
                    ? Enumerable.Repeat(_convolutionFactory.CreateSegment(start, end, Fraction.One), 1)
                    : Enumerable.Empty<Segment<TAlgebraicNumber>>();
        }
        
        private class SinglePoint<TAlgebraicNumberB> : Tracing<TAlgebraicNumberB>
        {
            public SinglePoint(IAlgebraicNumberCalculator<TAlgebraicNumberB> calculator,
                               Point<TAlgebraicNumberB> point,
                               Direction<TAlgebraicNumberB> tangent,
                               Fraction weight)
                : base(calculator, point, point, tangent, tangent, weight)
            {
                Point = point ?? throw new ArgumentNullException(nameof(point));
                Tangent = tangent ?? throw new ArgumentNullException(nameof(tangent));
            }

            public Point<TAlgebraicNumberB> Point { get; }

            public Direction<TAlgebraicNumberB> Tangent { get; }
        }
    }
}
