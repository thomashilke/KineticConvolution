using System;
using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution.Double
{
    internal static class ConvolutionHelper
    {
        public static IEnumerable<ConvolvedTracing<TAlgebraicNumber>> Convolve<TAlgebraicNumber>(
            Tracing<TAlgebraicNumber> tracing1,
            Tracing<TAlgebraicNumber> tracing2) where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber> =>
            (tracing1, tracing2) switch
            {
                (Arc<TAlgebraicNumber> arc1, Arc<TAlgebraicNumber> arc2) => ConvolveArcs(arc1, arc2),
                (Arc<TAlgebraicNumber> arc, Segment<TAlgebraicNumber> segment) => ConvolveArcAndSegment(arc, segment),
                (Segment<TAlgebraicNumber> segment, Arc<TAlgebraicNumber> arc) => ConvolveArcAndSegment(arc, segment),
                (Segment<TAlgebraicNumber> _, Segment<TAlgebraicNumber> _) =>
                    Enumerable.Empty<ConvolvedTracing<TAlgebraicNumber>>(),
                _ => throw new NotSupportedException(
                          "Only convolution between pairs of arcs and segments are supported, " +
                         $"but got a tracing of type {tracing1.GetType()} and a tracing of type " +
                         $"{tracing2.GetType()}.")
            };

        public static IEnumerable<ConvolvedTracing<TAlgebraicNumber>> ConvolveArcs<TAlgebraicNumber>(
            Arc<TAlgebraicNumber> arc1,
            Arc<TAlgebraicNumber> arc2) where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
        {
            if (arc1.Directions.Orientation == arc2.Directions.Orientation)
            {
                return arc1.Directions.Intersection(arc2.Directions)
                           .Select(
                               range =>
                                   Tracing<TAlgebraicNumber>.CreateArc(
                                       1,
                                       arc1.Center.Sum(arc2.Center),
                                       range,
                                       arc1.Radius.Add(arc2.Radius)))
                           .Select(arc => new ConvolvedTracing<TAlgebraicNumber>(arc, arc1, arc2));
            }

            return arc1.Directions.Intersection(arc2.Directions.Opposite())
                       .Select(
                           range =>
                               Tracing<TAlgebraicNumber>.CreateArc(
                                   1,
                                   arc1.Center.Sum(arc2.Center),
                                   range,
                                   arc1.Radius.Subtract(arc2.Radius)))
                       .Select(arc => new ConvolvedTracing<TAlgebraicNumber>(arc, arc1, arc2));
        }

        public static IEnumerable<ConvolvedTracing<TAlgebraicNumber>> ConvolveArcAndSegment<TAlgebraicNumber>(
            Arc<TAlgebraicNumber> arc,
            Segment<TAlgebraicNumber> segment) where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber> =>
            arc.Directions.Orientation switch
            {
                Orientation.CounterClockwise =>
                    segment.NormalDirection().Opposite().BelongsTo(arc.Directions)
                        ? new[]
                        {
                            new ConvolvedTracing<TAlgebraicNumber>(
                                Tracing<TAlgebraicNumber>.CreateSegment(
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
                                    segment.Start.Sum(arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    segment.End.Sum(arc.Center.Translate(segment.NormalDirection(), arc.Radius)),
                                    1),
                                arc,
                                segment)
                        }
                        : Enumerable.Empty<ConvolvedTracing<TAlgebraicNumber>>(),
                var orientation => throw new NotSupportedException(
                     "Only clockwise and counterclockwise arc orientations are supported, " +
                    $"but got {orientation}.")
            };
    }
}
