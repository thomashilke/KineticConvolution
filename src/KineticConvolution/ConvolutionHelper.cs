using System;
using System.Collections.Generic;
using System.Linq;

using static Hilke.KineticConvolution.Algorithms;

namespace Hilke.KineticConvolution
{
    public static class ConvolutionHelper
    {
        public static IEnumerable<ConvolvedTracing> Convolve(Tracing tracing1, Tracing tracing2) =>
            (tracing1, tracing2) switch
            {
                (Arc arc1, Arc arc2) => ConvolveArcs(arc1, arc2),
                (Arc arc, Segment segment) => ConvolveArcAndSegment(arc, segment),
                (Segment segment, Arc arc) => ConvolveArcAndSegment(arc, segment),
                (Segment _, Segment _) => Enumerable.Empty<ConvolvedTracing>(),
                _ => throw new NotSupportedException()
            };

        public static IEnumerable<ConvolvedTracing> ConvolveArcs(Arc arc1, Arc arc2)
        {
            if (arc1.Directions.Orientation == arc2.Directions.Orientation)
            {
                return Intersection(arc1.Directions, arc2.Directions)
                       .Select(
                           range =>
                               Tracing.CreateArc(1, Sum(arc1.Center, arc2.Center), range, arc1.Radius.Add(arc2.Radius)))
                       .Select(arc => new ConvolvedTracing(arc, arc1, arc2));
            }

            return Intersection(arc1.Directions, arc2.Directions.Opposite())
                   .Select(
                       range =>
                           Tracing.CreateArc(
                               1,
                               Sum(arc1.Center, arc2.Center),
                               range,
                               arc1.Radius.Subtract(arc2.Radius)))
                   .Select(arc => new ConvolvedTracing(arc, arc1, arc2));
        }

        public static IEnumerable<ConvolvedTracing> ConvolveArcAndSegment(Arc arc, Segment segment) =>
            arc.Directions.Orientation switch
            {
                Orientation.CounterClockwise =>
                    segment.NormalDirection().Opposite().BelongsTo(arc.Directions)
                        ? new[]
                        {
                            new ConvolvedTracing(
                                Tracing.CreateSegment(
                                    Sum(
                                        segment.Start,
                                        arc.Center.Translate(segment.NormalDirection().Opposite(), arc.Radius)),
                                    Sum(
                                        segment.End,
                                        arc.Center.Translate(segment.NormalDirection().Opposite(), arc.Radius)),
                                    1),
                                arc,
                                segment)
                        }
                        : Enumerable.Empty<ConvolvedTracing>(),
                Orientation.Clockwise =>
                    segment.NormalDirection().BelongsTo(arc.Directions)
                        ? new[]
                        {
                            new ConvolvedTracing(
                                Tracing.CreateSegment(
                                    Sum(
                                        segment.Start,
                                        arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    Sum(
                                        segment.End,
                                        arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    1),
                                arc,
                                segment)
                        }
                        : Enumerable.Empty<ConvolvedTracing>(),
                _ => throw new NotSupportedException()
            };
    }
}
