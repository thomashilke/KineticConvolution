using System;
using System.Collections.Generic;
using System.Linq;

using Fractions;

namespace Hilke.KineticConvolution
{
    public class ConvolutionFactory<TAlgebraicNumber>
        where TAlgebraicNumber : IEquatable<TAlgebraicNumber>
    {
        public ConvolutionFactory(IAlgebraicNumberCalculator<TAlgebraicNumber> algebraicNumberCalculator) =>
            AlgebraicNumberCalculator = algebraicNumberCalculator
                                     ?? throw new ArgumentNullException(nameof(algebraicNumberCalculator));

        public IAlgebraicNumberCalculator<TAlgebraicNumber> AlgebraicNumberCalculator { get; }

        public Point<TAlgebraicNumber> CreatePoint(TAlgebraicNumber x, TAlgebraicNumber y) =>
            new Point<TAlgebraicNumber>(AlgebraicNumberCalculator, x, y);
        
        public Segment<TAlgebraicNumber> CreateSegment(
            IAlgebraicNumberCalculator<TAlgebraicNumber> calculator,
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Fraction weight)
        {
            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (end == null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            if (start == end)
            {
                throw new ArgumentException("The start point cannot be the same as end point.", nameof(start));
            }

            var direction = start.DirectionTo(end);
            return new Segment<TAlgebraicNumber>(calculator, start, end, direction, direction, weight);
        }

        public Arc<TAlgebraicNumber> CreateArc(
            Fraction weight,
            Point<TAlgebraicNumber> center,
            DirectionRange<TAlgebraicNumber> directions,
            Orientation orientation,
            TAlgebraicNumber radius)
        {
            if (center == null)
            {
                throw new ArgumentNullException(nameof(center));
            }

            if (directions == null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            if (radius == null)
            {
                throw new ArgumentNullException(nameof(radius));
            }

            var start = center.Translate(directions.Start, radius);
            var end = center.Translate(directions.End, radius);

            var startNormalDirection = directions.Start.NormalDirection();
            var startDirection = directions.Orientation == Orientation.Clockwise
                                     ? startNormalDirection.Opposite()
                                     : startNormalDirection;

            var endNormalDirection = directions.End.NormalDirection();
            var endDirection = directions.Orientation == Orientation.Clockwise
                                   ? endNormalDirection.Opposite()
                                   : endNormalDirection;

            return new Arc<TAlgebraicNumber>(
                AlgebraicNumberCalculator,
                weight,
                center,
                directions,
                radius,
                start,
                end,
                startDirection,
                endDirection);
        }

        public Convolution<TAlgebraicNumber> FromShapes(
            Shape<TAlgebraicNumber> shape1,
            Shape<TAlgebraicNumber> shape2)
        {
            var convolutions =
                from tracing1 in shape1.Tracings
                from tracing2 in shape2.Tracings
                select Convolve(tracing1, tracing2);

            return new Convolution<TAlgebraicNumber>(shape1, shape2, convolutions.SelectMany(x => x).ToList());
        }

        private IEnumerable<ConvolvedTracing<TAlgebraicNumber>> Convolve(
            Tracing<TAlgebraicNumber> tracing1,
            Tracing<TAlgebraicNumber> tracing2) =>
            (tracing1, tracing2) switch
            {
                (Arc<TAlgebraicNumber> arc1, Arc<TAlgebraicNumber> arc2) => ConvolveArcs(arc1, arc2),
                (Arc<TAlgebraicNumber> arc, Segment<TAlgebraicNumber> segment) => ConvolveArcAndSegment(arc, segment),
                (Segment<TAlgebraicNumber> segment, Arc<TAlgebraicNumber> arc) => ConvolveArcAndSegment(arc, segment),
                (Segment<TAlgebraicNumber> _, Segment<TAlgebraicNumber> _) =>
                    Enumerable.Empty<ConvolvedTracing<TAlgebraicNumber>>(),
                _ => throw new NotSupportedException(
                         "Only convolution between pairs of arcs and segments are supported, "
                       + $"but got a tracing of type {tracing1.GetType()} and a tracing of type "
                       + $"{tracing2.GetType()}.")
            };

        private IEnumerable<ConvolvedTracing<TAlgebraicNumber>> ConvolveArcs(
            Arc<TAlgebraicNumber> arc1,
            Arc<TAlgebraicNumber> arc2)
        {
            if (arc1.Directions.Orientation == arc2.Directions.Orientation)
            {
                return arc1.Directions.Intersection(arc2.Directions)
                           .Select(
                               range =>
                                   Tracing<TAlgebraicNumber>.CreateArc(
                                       AlgebraicNumberCalculator,
                                       1,
                                       arc1.Center.Sum(arc2.Center),
                                       range,
                                       AlgebraicNumberCalculator.Add(arc1.Radius, arc2.Radius)))
                           .Select(arc => new ConvolvedTracing<TAlgebraicNumber>(arc, arc1, arc2));
            }

            return arc1.Directions.Intersection(arc2.Directions.Opposite())
                       .Select(
                           range =>
                               Tracing<TAlgebraicNumber>.CreateArc(
                                   AlgebraicNumberCalculator,
                                   1,
                                   arc1.Center.Sum(arc2.Center),
                                   range,
                                   AlgebraicNumberCalculator.Subtract(arc1.Radius, arc2.Radius)))
                       .Select(arc => new ConvolvedTracing<TAlgebraicNumber>(arc, arc1, arc2));
        }

        private IEnumerable<ConvolvedTracing<TAlgebraicNumber>> ConvolveArcAndSegment(
            Arc<TAlgebraicNumber> arc,
            Segment<TAlgebraicNumber> segment) =>
            arc.Directions.Orientation switch
            {
                Orientation.CounterClockwise =>
                    segment.NormalDirection().Opposite().BelongsTo(arc.Directions)
                        ? new[]
                        {
                            new ConvolvedTracing<TAlgebraicNumber>(
                                Tracing<TAlgebraicNumber>.CreateSegment(
                                    AlgebraicNumberCalculator,
                                    segment.Start.Sum(
                                        arc.Center.Translate(segment.NormalDirection().Opposite(), arc.Radius)),
                                    segment.End.Sum(
                                        arc.Center.Translate(segment.NormalDirection().Opposite(), arc.Radius)),
                                    1),
                                arc,
                                segment)
                        }
                        : Enumerable.Empty<ConvolvedTracing<TAlgebraicNumber>>(),
                Orientation.Clockwise =>
                    segment.NormalDirection().BelongsTo(arc.Directions)
                        ? new[]
                        {
                            new ConvolvedTracing<TAlgebraicNumber>(
                                Tracing<TAlgebraicNumber>.CreateSegment(
                                    AlgebraicNumberCalculator,
                                    segment.Start.Sum(arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    segment.End.Sum(arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    1),
                                arc,
                                segment)
                        }
                        : Enumerable.Empty<ConvolvedTracing<TAlgebraicNumber>>(),
                var orientation => throw new NotSupportedException(
                                       "Only clockwise and counterclockwise arc orientations are supported, "
                                     + $"but got {orientation}.")
            };
    }
}
