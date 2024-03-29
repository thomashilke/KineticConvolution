﻿using System;
using System.Collections.Generic;
using System.Linq;

using Fractions;

using Hilke.KineticConvolution.Extensions;

namespace Hilke.KineticConvolution
{
    public class ConvolutionFactory<TAlgebraicNumber> : IConvolutionFactory<TAlgebraicNumber>
    {
        public ConvolutionFactory(IAlgebraicNumberCalculator<TAlgebraicNumber> algebraicNumberCalculator)
        {
            AlgebraicNumberCalculator = algebraicNumberCalculator
                                     ?? throw new ArgumentNullException(nameof(algebraicNumberCalculator));

            Zero = algebraicNumberCalculator.FromInt(0);

            One = algebraicNumberCalculator.FromInt(1);
        }

        public IAlgebraicNumberCalculator<TAlgebraicNumber> AlgebraicNumberCalculator { get; }

        public TAlgebraicNumber Zero { get; }

        public TAlgebraicNumber One { get; }

        public Point<TAlgebraicNumber> CreatePoint(TAlgebraicNumber x, TAlgebraicNumber y) =>
            new Point<TAlgebraicNumber>(AlgebraicNumberCalculator, x, y);

        public Direction<TAlgebraicNumber> CreateDirection(TAlgebraicNumber x, TAlgebraicNumber y) =>
            new Direction<TAlgebraicNumber>(AlgebraicNumberCalculator, x, y);

        public DirectionRange<TAlgebraicNumber> CreateDirectionRange(
            Direction<TAlgebraicNumber> start,
            Direction<TAlgebraicNumber> end,
            Orientation orientation) =>
            new DirectionRange<TAlgebraicNumber>(AlgebraicNumberCalculator, start, end, orientation);

        public Segment<TAlgebraicNumber> CreateSegment(
            Point<TAlgebraicNumber> start,
            Point<TAlgebraicNumber> end,
            Fraction weight)
        {
            if (start is null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (end is null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            if (start == end)
            {
                throw new ArgumentException("The start point cannot be the same as end point.", nameof(start));
            }

            var direction = start.DirectionTo(end);
            return new Segment<TAlgebraicNumber>(AlgebraicNumberCalculator, start, end, direction, direction, weight);
        }

        public Segment<TAlgebraicNumber> CreateSegment(
            TAlgebraicNumber startX,
            TAlgebraicNumber startY,
            TAlgebraicNumber endX,
            TAlgebraicNumber endY,
            Fraction weight) =>
            CreateSegment(
                CreatePoint(startX, startY),
                CreatePoint(endX, endY),
                weight);

        public Arc<TAlgebraicNumber> CreateArc(
            Point<TAlgebraicNumber> center,
            DirectionRange<TAlgebraicNumber> directions,
            TAlgebraicNumber radius,
            Fraction weight)
        {
            if (center is null)
            {
                throw new ArgumentNullException(nameof(center));
            }

            if (directions is null)
            {
                throw new ArgumentNullException(nameof(directions));
            }

            if (radius is null)
            {
                throw new ArgumentNullException(nameof(radius));
            }

            var start = center.Translate(directions.Start, radius);
            var end = center.Translate(directions.End, radius);

            var startNormalDirection = directions.Start.NormalDirection();
            var startTangentDirection = directions.Orientation == Orientation.Clockwise
                ? startNormalDirection.Opposite()
                : startNormalDirection;

            var endNormalDirection = directions.End.NormalDirection();
            var endTangentDirection = directions.Orientation == Orientation.Clockwise
                ? endNormalDirection.Opposite()
                : endNormalDirection;

            return new Arc<TAlgebraicNumber>(
                AlgebraicNumberCalculator,
                center,
                directions,
                radius,
                start,
                end,
                startTangentDirection,
                endTangentDirection,
                weight);
        }

        public Arc<TAlgebraicNumber> CreateArc(
            TAlgebraicNumber centerX,
            TAlgebraicNumber centerY,
            TAlgebraicNumber directionStartX,
            TAlgebraicNumber directionStartY,
            TAlgebraicNumber directionEndX,
            TAlgebraicNumber directionEndY,
            Orientation orientation,
            TAlgebraicNumber radius,
            Fraction weight) =>
            CreateArc(
                CreatePoint(centerX, centerY),
                CreateDirectionRange(
                    CreateDirection(directionStartX, directionStartY),
                    CreateDirection(directionEndX, directionEndY),
                    orientation),
                radius,
                weight);

        public Convolution<TAlgebraicNumber> ConvolveShapes(
            Shape<TAlgebraicNumber> shape1,
            Shape<TAlgebraicNumber> shape2)
        {
            var convolutions =
                from tracing1 in shape1.Tracings
                from tracing2 in shape2.Tracings
                select ConvolveTracings(tracing1, tracing2);

            return new Convolution<TAlgebraicNumber>(shape1, shape2, convolutions.SelectMany(x => x).ToList());
        }

        public Shape<TAlgebraicNumber> CreateShape(IEnumerable<Tracing<TAlgebraicNumber>> tracings)
        {
            var tracingsEnumerated = tracings?.ToList() ?? throw new ArgumentNullException(nameof(tracings));

            if (tracingsEnumerated.Count < 1)
            {
                throw new ArgumentException("There should be at least one tracing.", nameof(tracings));
            }

            if (!verifyConditionOnConsecutive(tracingsEnumerated, continuityCondition, out var discontinuityLocations))
            {
                throw new ArgumentException(
                    $"The tracings must be continuous. Discontinuity detected between tracings "
                  + $"{string.Join(", ", discontinuityLocations.Select(index => $"{index} -> {nextIndex(index)}"))}",
                    nameof(tracings));
            }

            if (!verifyConditionOnConsecutive(tracingsEnumerated, tangentContinuityCondition, out var tangentDiscontinuityLocations))
            {
                throw new ArgumentException(
                    $"The tracings tangents must be continuous. Tangents discontinuity detected between tracings "
                  + $"{string.Join(", ", tangentDiscontinuityLocations.Select(index => $"{index} -> {nextIndex(index)}"))}",
                    nameof(tracings));
            }

            return new Shape<TAlgebraicNumber>(tracingsEnumerated);

            static bool continuityCondition(Tracing<TAlgebraicNumber> current, Tracing<TAlgebraicNumber> next) =>
                current.IsContinuousWith(next);

            static bool tangentContinuityCondition(Tracing<TAlgebraicNumber> current, Tracing<TAlgebraicNumber> next) =>
                current.TangentIsContinuousWith(next);

            static bool verifyConditionOnConsecutive(IReadOnlyList<Tracing<TAlgebraicNumber>> tracings,
                                    Func<Tracing<TAlgebraicNumber>, Tracing<TAlgebraicNumber>, bool> condition,
                                     out IReadOnlyList<int> indicesWhereConditionFails)
            {
                indicesWhereConditionFails = tracings.CyclicPairwise(condition)
                                                     .Select((satisfiesCondition, index) => (satisfiesCondition, index))
                                                     .Where(e => !e.satisfiesCondition)
                                                     .Select(e => e.index)
                                                     .ToList();

                return indicesWhereConditionFails.Count == 0;
            }

            int nextIndex(int index) => index != tracingsEnumerated.Count - 1 ? index + 1 : 0;
        }

        public IEnumerable<ConvolvedTracing<TAlgebraicNumber>> ConvolveTracings(
            Tracing<TAlgebraicNumber> tracing1,
            Tracing<TAlgebraicNumber> tracing2) =>
            (tracing1, tracing2) switch
            {
                (Arc<TAlgebraicNumber> arc1, Arc<TAlgebraicNumber> arc2) => ConvolveArcs(arc1, arc2),

                (Arc<TAlgebraicNumber> arc, Segment<TAlgebraicNumber> segment) =>
                    ConvolveArcAndSegment(arc, segment)
                        .Select(tracing => new ConvolvedTracing<TAlgebraicNumber>(tracing, arc, segment)),

                (Segment<TAlgebraicNumber> segment, Arc<TAlgebraicNumber> arc) =>
                    ConvolveArcAndSegment(arc, segment)
                        .Select(tracing => new ConvolvedTracing<TAlgebraicNumber>(tracing, segment, arc)),

                (Segment<TAlgebraicNumber> _, Segment<TAlgebraicNumber> _) =>
                    Enumerable.Empty<ConvolvedTracing<TAlgebraicNumber>>(),

                _ => throw new NotSupportedException(
                         "Only convolution between pairs of arcs and segments are supported, "
                       + $"but got a tracing of type {tracing1.GetType()} and a tracing of type "
                       + $"{tracing2.GetType()}.")
            };

        internal IEnumerable<ConvolvedTracing<TAlgebraicNumber>> ConvolveArcs(
            Arc<TAlgebraicNumber> arc1,
            Arc<TAlgebraicNumber> arc2)
        {
            if (arc1.Directions.Orientation == arc2.Directions.Orientation)
            {
                var convolutionWeightSign = arc1.Directions.Orientation == Orientation.CounterClockwise ? 1 : -1;
                var convolutionWeight = convolutionWeightSign * arc1.Weight * arc2.Weight;

                return arc1.Directions.Intersection(arc2.Directions)
                           .Select(
                               range => CreateArc(
                                   arc1.Center.Sum(arc2.Center),
                                   range,
                                   AlgebraicNumberCalculator.Add(arc1.Radius, arc2.Radius),
                                   convolutionWeight))
                           .Select(arc => new ConvolvedTracing<TAlgebraicNumber>(arc, arc1, arc2));
            }

            return arc1.Directions.Intersection(arc2.Directions.Opposite())
                .Select(range =>
                {
                    var signedRadius = AlgebraicNumberCalculator.Subtract(arc1.Radius, arc2.Radius);

                    var convolutionWeightSign =
                        (arc1.Directions.Orientation == Orientation.Clockwise && arc2.Directions.Orientation == Orientation.CounterClockwise);
                    var convolutionWeight = (convolutionWeightSign ? 1 : -1) * arc1.Weight * arc2.Weight;

                    return CreateArc(
                        arc1.Center.Sum(arc2.Center),
                        AlgebraicNumberCalculator.IsStrictlyNegative(signedRadius)
                            ? range.Opposite()
                            : range,
                        AlgebraicNumberCalculator.Abs(signedRadius),
                        convolutionWeight);
                })
                .Select(arc => new ConvolvedTracing<TAlgebraicNumber>(arc, arc1, arc2));
        }

        internal IEnumerable<Tracing<TAlgebraicNumber>> ConvolveArcAndSegment(
            Arc<TAlgebraicNumber> arc,
            Segment<TAlgebraicNumber> segment)
        {
            var segmentNormalDirection = arc.Directions.Orientation switch
                {
                    Orientation.CounterClockwise => segment.NormalDirection.Opposite(),

                    Orientation.Clockwise => segment.NormalDirection,

                    _ => throw new NotSupportedException(
                        "Only clockwise and counterclockwise arc orientations are supported, "
                        + $"but got {arc.Directions.Orientation}.")
                };

            var segmentNormalDirectionIsStart = segmentNormalDirection == arc.Directions.Start;
            var segmentNormalDirectionIsEnd = segmentNormalDirection == arc.Directions.End;

            var isSegmentConvolvedWithOneArcExtremity =
                segmentNormalDirectionIsStart ^ segmentNormalDirectionIsEnd;

            var weightSign = arc.Directions.Orientation == Orientation.CounterClockwise
                ? 1 : -1;

            var convolutionWeight = isSegmentConvolvedWithOneArcExtremity
                ? new Fraction(1, 2) * weightSign * arc.Weight * segment.Weight
                : weightSign * arc.Weight * segment.Weight;

            if (segmentNormalDirection.BelongsTo(arc.Directions))
            {
                return new[]
                {
                    CreateSegment(
                        segment.Start.Sum(arc.Center.Translate(segmentNormalDirection, arc.Radius)),
                        segment.End.Sum(arc.Center.Translate(segmentNormalDirection, arc.Radius)),
                        convolutionWeight),
                };
            }

            return Enumerable.Empty<Tracing<TAlgebraicNumber>>();
        }
    }
}
