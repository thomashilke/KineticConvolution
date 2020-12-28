using System;
using System.Collections.Generic;
using System.Linq;

using static KineticConvolution.Algorithms;

namespace KineticConvolution
{
    public class ConvolvedTracing
    {
        public ConvolvedTracing(Tracing convolution, Tracing parent1, Tracing parent2)
        {
            Convolution = convolution ?? throw new ArgumentNullException(nameof(convolution));

            Parent1 = parent1 ?? throw new ArgumentNullException(nameof(parent1));

            Parent2 = parent2 ?? throw new ArgumentNullException(nameof(parent2));
        }

        public Tracing Parent1 { get; }

        public Tracing Parent2 { get; }

        public Tracing Convolution { get; }
    }

    public class Convolution
    {
        public Convolution(IEnumerable<Tracing> shape1, IEnumerable<Tracing> shape2)
        {
            Shape1 = shape1 ?? throw new ArgumentNullException(nameof(shape1));

            Shape2 = shape2 ?? throw new ArgumentNullException(nameof(shape2));

            var convolutions =
                from tracing1 in shape1
                from tracing2 in shape2
                select Convolve(tracing1, tracing2);

            ConvolvedTracings = convolutions.SelectMany(x => x);
        }

        public IEnumerable<Tracing> Shape1 { get; }

        public IEnumerable<Tracing> Shape2 { get; }

        public IEnumerable<ConvolvedTracing> ConvolvedTracings { get; }

        private IEnumerable<ConvolvedTracing> Convolve(Tracing tracing1, Tracing tracing2) =>
            (tracing1, tracing2) switch
            {
                (Arc arc1, Arc arc2) => ConvolveArcs(arc1, arc2),
                (Arc arc, Segment segment) => ConvolveArcAndSegment(arc, segment),
                (Segment segment, Arc arc) => ConvolveArcAndSegment(arc, segment),
                (Segment _, Segment _) => Enumerable.Empty<ConvolvedTracing>(),
                _ => throw new NotSupportedException()
            };

        private static IEnumerable<ConvolvedTracing> ConvolveArcs(Arc arc1, Arc arc2)
        {
            if (arc1.Directions.Orientation == arc2.Directions.Orientation)
            {
                return Intersection(arc1.Directions, arc2.Directions)
                    .Select(range =>
                            new Arc(1, Sum(arc1.Center, arc2.Center), range, arc1.Radius.Add(arc2.Radius)))
                    .Select(arc => new ConvolvedTracing(arc, arc1, arc2));
            }
            else
            {
                return Intersection(arc1.Directions, arc2.Directions.Opposite())
                    .Select(range =>
                            new Arc(1, Sum(arc1.Center, arc2.Center), range, arc1.Radius.Substract(arc2.Radius)))
                    .Select(arc => new ConvolvedTracing(arc, arc1, arc2));
            }
        }

        private static IEnumerable<ConvolvedTracing> ConvolveArcAndSegment(Arc arc, Segment segment)
        {
            switch (arc.Directions.Orientation)
            {
                case Orientation.CounterClockwise:
                    return segment.NormalDirection().Opposite().BelongsTo(arc.Directions)
                        ? new [] { new ConvolvedTracing(
                            new Segment(
                                1,
                                Sum(
                                    segment.Start,
                                    arc.Center.Translate(segment.NormalDirection().Opposite(), arc.Radius)),
                                Sum(
                                    segment.End,
                                    arc.Center.Translate(segment.NormalDirection().Opposite(), arc.Radius))),
                            arc, segment) }
                        : Enumerable.Empty<ConvolvedTracing>();

                case Orientation.Clockwise:
                    return segment.NormalDirection().BelongsTo(arc.Directions)
                        ? new [] { new ConvolvedTracing(
                            new Segment(
                                1,
                                Sum(
                                    segment.Start,
                                    arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                Sum(
                                    segment.End,
                                    arc.Center.Translate(segment.NormalDirection(), arc.Radius))),
                            arc, segment) }
                        : Enumerable.Empty<ConvolvedTracing>();

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
