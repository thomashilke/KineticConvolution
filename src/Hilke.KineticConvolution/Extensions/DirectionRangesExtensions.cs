using System;
using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution.Extensions
{
    public static class DirectionRangesExtensions
    {
        public static IEnumerable<DirectionRange<double>> Union(
            this IEnumerable<DirectionRange<double>> ranges)
        {
            if (ranges is null)
            {
                throw new ArgumentNullException(nameof(ranges));
            }

            var orderedRanges = ranges.SortCounterClockwise().ToList();
            if(!orderedRanges.Any())
            {
                return Enumerable.Empty<DirectionRange<double>>();
            }

            var disjointUnions = new Queue<DirectionRange<double>>();
            var stagingUnion = orderedRanges[0];
            foreach (var currentRange in orderedRanges)
            {
                var unionWithStaging = stagingUnion.Union(currentRange).ToList();
                if(unionWithStaging.Count == 2)
                {
                    disjointUnions.Enqueue(stagingUnion);
                    stagingUnion = currentRange;
                }
                else
                {
                    stagingUnion = unionWithStaging.Single();
                }
            }

            if (disjointUnions.Peek().Start.BelongsTo(stagingUnion))
            {
                // The last stagingUnion still intersect some ranges
                // at the front of the queue.
                var lastIntersectingUnion = disjointUnions.Peek();
                while (disjointUnions.Count > 0)
                {
                    if (disjointUnions.Peek().Start.BelongsTo(stagingUnion))
                    {
                        lastIntersectingUnion = disjointUnions.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }

                stagingUnion = stagingUnion.Union(lastIntersectingUnion).Single();
            }

            disjointUnions.Enqueue(stagingUnion);
            return disjointUnions;
        }

        public static IEnumerable<DirectionRange<TAlgebraicNumber>> SortCounterClockwise<TAlgebraicNumber>(
            this IEnumerable<DirectionRange<TAlgebraicNumber>> ranges)
        {
            if (ranges is null)
            {
                throw new ArgumentNullException(nameof(ranges));
            }

            var enumeratedRanges = ranges.ToList();

            if (enumeratedRanges.Any())
            {
                return SortCounterClockwiseWithRespectTo(
                    ranges: enumeratedRanges,
                    referenceDirection: enumeratedRanges[0].Start);
            }
            else
            {
                return Enumerable.Empty<DirectionRange<TAlgebraicNumber>>();
            }
        }

        public static IEnumerable<DirectionRange<TAlgebraicNumber>> SortCounterClockwiseWithRespectTo<TAlgebraicNumber>(
            this IEnumerable<DirectionRange<TAlgebraicNumber>> ranges,
            Direction<TAlgebraicNumber> referenceDirection)
        {
            if (ranges is null)
            {
                throw new ArgumentNullException(nameof(ranges));
            }

            if (referenceDirection is null)
            {
                throw new ArgumentNullException(nameof(referenceDirection));
            }

            var enumeratedRanges = ranges.ToList();
            if (enumeratedRanges.Any(range => range.Orientation != Orientation.CounterClockwise))
            {
                throw new NotSupportedException("Only counterclockwise ranges can be ordered.");
            }

            enumeratedRanges.Sort((range1, range2) => (int)range1.Start.CompareTo(range2.Start, referenceDirection));

            return enumeratedRanges;
        }
    }
}
