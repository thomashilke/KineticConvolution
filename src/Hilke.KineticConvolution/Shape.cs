using System.Collections.Generic;
using System.Linq;

namespace Hilke.KineticConvolution
{
    public sealed class Shape<TAlgebraicNumber>
    {
        internal Shape(IReadOnlyList<Tracing<TAlgebraicNumber>> tracings) => Tracings = tracings;

        public IReadOnlyList<Tracing<TAlgebraicNumber>> Tracings { get; }

        public bool IsG1Continuous() =>
            Tracings
                .Zip(
                    Tracings.Skip(1).Concat(new[] {Tracings.First()}),
                    (right, left) => right.IsG1ContinuousWith(left))
                .All(isContinuous => isContinuous);
    }
}
