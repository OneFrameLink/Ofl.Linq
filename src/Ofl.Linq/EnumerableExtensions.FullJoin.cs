using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<(TOuter Outer, TInner Inner)> FullJoin<TOuter, TInner, TKey>(
            this IEnumerable<TOuter> outer, IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector) =>
            outer.FullJoin(inner, outerKeySelector, innerKeySelector, EqualityComparer<TKey>.Default);

        public static IEnumerable<(TOuter Outer, TInner Inner)> FullJoin<TOuter, TInner, TKey>(
            this IEnumerable<TOuter> outer, IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> equalityComparer)
        {
            // Validate parameters.
            if (outer == null) throw new ArgumentNullException(nameof(outer));
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Materialize the inner.
            IDictionary<TKey, TInner> mappedRight = inner.ToDictionary(innerKeySelector, equalityComparer);

            // The implementation.
            IEnumerable<(TOuter, TInner)> Implementation()
            {
                // Cycle through the items.
                foreach (TOuter leftItem in outer)
                {
                    // The key.
                    TKey key = outerKeySelector(leftItem);

                    // Look up the item.
                    if (mappedRight.TryGetValue(key, out TInner rightItem))
                    {
                        // Yield the pair.
                        yield return (leftItem, rightItem);

                        // Remove the inner item from the dictionary.
                        mappedRight.Remove(key);
                    }
                    else
                        // Yield just the outer.
                        yield return (leftItem, default);
                }

                // Yield the remaining inner items, outer for null.
                foreach (TInner rightItem in mappedRight.Values)
                    yield return (default, rightItem);
            }

            // Return.
            return Implementation();
        }
    }
}