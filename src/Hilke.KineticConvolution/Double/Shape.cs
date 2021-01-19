using System;
using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution.Double
{
    public class Shape<TAlgebraicNumber> where TAlgebraicNumber : IAlgebraicNumber<TAlgebraicNumber>
    {
        private Shape(IReadOnlyList<Tracing<TAlgebraicNumber>> tracings) => Tracings = tracings;

        public IReadOnlyList<Tracing<TAlgebraicNumber>> Tracings { get; }

        public static Shape<TAlgebraicNumber> FromTracings(IEnumerable<Tracing<TAlgebraicNumber>> tracings)
        {
            var tracingsEnumerated = tracings?.ToList() ?? throw new ArgumentNullException(nameof(tracings));

            if (tracingsEnumerated.Count < 1)
            {
                throw new ArgumentException("There should be at least one tracing.", nameof(tracings));
            }

            if (!IsG1Continuous(tracingsEnumerated))
            {
                throw new ArgumentException("The tracings should be continuous.", nameof(tracings));
            }

            return new Shape<TAlgebraicNumber>(tracingsEnumerated);
        }

        private static bool IsG1Continuous(IReadOnlyList<Tracing<TAlgebraicNumber>> tracings) =>
            tracings.Zip(
                        tracings.Skip(1).Concat(new[] {tracings.First()}),
                        (right, left) => right.IsG1ContinuousWith(left))
                    .All(isContinuous => isContinuous);
    }
}
