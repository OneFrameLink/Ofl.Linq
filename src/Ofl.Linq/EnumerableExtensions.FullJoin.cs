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

            // Get a dictionary of groupings.
            IDictionary<TKey, TInner[]> innerLookup = inner
                .GroupBy(innerKeySelector, equalityComparer)
                .ToDictionary(g => g.Key, g => g.ToArray(), equalityComparer);

            // The set of keys that were hit.
            ISet<TKey> matchedKeys = new HashSet<TKey>(equalityComparer);

            // The implementation.
            IEnumerable<(TOuter, TInner)> Implementation() {
                // Cycle through the items.
                foreach (TOuter outerItem in outer)
                {
                    // The key.
                    TKey key = outerKeySelector(outerItem);

                    // Try and get the array of items.
                    if (innerLookup.TryGetValue(key, out TInner[] innerArray))
                    {
                        // Yield the entire sequence for the left.
                        foreach (TInner innerItem in innerArray)
                            yield return (outerItem, innerItem);

                        // Add the item to the matched keys.
                        matchedKeys.Add(key);
                    }
                    else
                        // Yield just the outer item with the default.
                        yield return (outerItem, default);
                }

                // The remainder.
                // Everything in the inner lookup keys that
                // has not been matched.
                IEnumerable<TInner> remainingItems = innerLookup.Keys
                    .Where(k => !matchedKeys.Contains(k))
                    .SelectMany(k => innerLookup[k]);

                // Yield.
                foreach (TInner remainingItem in remainingItems)
                    yield return (default, remainingItem);
            }

            // Return.
            return Implementation();
        }
    }
}