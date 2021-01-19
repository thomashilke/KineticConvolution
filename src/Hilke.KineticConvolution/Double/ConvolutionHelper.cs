using System;
using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution.Double
{
    internal static class ConvolutionHelper
    {
        public static IEnumerable<ConvolvedTracing> Convolve(
            Tracing tracing1,
            Tracing tracing2) =>
            (tracing1, tracing2) switch
            {
                (Arc arc1, Arc arc2) => ConvolveArcs(arc1, arc2),
                (Arc arc, Segment segment) => ConvolveArcAndSegment(arc, segment),
                (Segment segment, Arc arc) => ConvolveArcAndSegment(arc, segment),
                (Segment _, Segment _) =>
                    Enumerable.Empty<ConvolvedTracing>(),
                _ => throw new NotSupportedException(
                          "Only convolution between pairs of arcs and segments are supported, " +
                         $"but got a tracing of type {tracing1.GetType()} and a tracing of type " +
                         $"{tracing2.GetType()}.")
            };

        public static IEnumerable<ConvolvedTracing> ConvolveArcs(
            Arc arc1,
            Arc arc2)
        {
            if (arc1.Directions.Orientation == arc2.Directions.Orientation)
            {
                return arc1.Directions.Intersection(arc2.Directions)
                           .Select(
                               range =>
                                   Tracing.CreateArc(
                                       1,
                                       arc1.Center.Sum(arc2.Center),
                                       range,
                                       arc1.Radius + arc2.Radius))
                           .Select(arc => new ConvolvedTracing(arc, arc1, arc2));
            }

            return arc1.Directions.Intersection(arc2.Directions.Opposite())
                       .Select(
                           range =>
                               Tracing.CreateArc(
                                   1,
                                   arc1.Center.Sum(arc2.Center),
                                   range,
                                   arc1.Radius - arc2.Radius))
                       .Select(arc => new ConvolvedTracing(arc, arc1, arc2));
        }

        public static IEnumerable<ConvolvedTracing> ConvolveArcAndSegment(
            Arc arc,
            Segment segment) =>
            arc.Directions.Orientation switch
            {
                Orientation.CounterClockwise =>
                    segment.NormalDirection().Opposite().BelongsTo(arc.Directions)
                        ? new[]
                        {
                            new ConvolvedTracing(
                                Tracing.CreateSegment(
                                    segment.Start.Sum(
                                        arc.Center.Translate(segment.NormalDirection().Opposite(), arc.Radius)),
                                    segment.End.Sum(
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
                                    segment.Start.Sum(arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    segment.End.Sum(arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    1),
                                arc,
                                segment)
                        }
                        : Enumerable.Empty<ConvolvedTracing>(),
                var orientation => throw new NotSupportedException(
                     "Only clockwise and counterclockwise arc orientations are supported, " +
                    $"but got {orientation}.")
            };
    }
}
