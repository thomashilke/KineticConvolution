using System;
using System.Collections.Generic;

namespace Hilke.KineticConvolution.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<TSelected> CyclicPairwise<TElement, TSelected>(
            this IEnumerable<TElement> source,
            Func<TElement, TElement, TSelected> selector)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return cyclicPairwiseIterator();

            IEnumerable<TSelected> cyclicPairwiseIterator()
            {
                using var enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                {
                    yield break;
                }

                var firstElement = enumerator.Current;
                var previousElement = enumerator.Current;
                var closeCycle = false;

                while (enumerator.MoveNext())
                {
                    closeCycle = true;
                    var nextElement = enumerator.Current;

                    yield return selector(previousElement, nextElement);

                    previousElement = nextElement;
                }

                if (closeCycle)
                {
                    yield return selector(previousElement, firstElement);
                }
            }
        }
    }
}
