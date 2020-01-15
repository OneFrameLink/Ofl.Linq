using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<(TOuter Outer, TInner Inner)> FullJoin<TOuter, TInner, TKey>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector
        ) =>
            outer.FullJoin(
                inner,
                outerKeySelector,
                innerKeySelector,
                default,
                default,
                EqualityComparer<TKey>.Default
            );

        public static IEnumerable<(TOuter Outer, TInner Inner)> FullJoin<TOuter, TInner, TKey>(
            this IEnumerable<TOuter> outer, 
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            [AllowNull]
            TInner unmatchedInner,
            [AllowNull]
            TOuter unmatchedOuter
        ) =>
            outer.FullJoin(
                inner, 
                outerKeySelector, 
                innerKeySelector, 
                unmatchedInner, 
                unmatchedOuter,
                EqualityComparer<TKey>.Default
            );

        public static IEnumerable<(TOuter Outer, TInner Inner)> FullJoin<TOuter, TInner, TKey>(
            this IEnumerable<TOuter> outer, 
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector, 
            Func<TInner, TKey> innerKeySelector,
            TInner unmatchedInner,
            TOuter unmatchedOuter,
            IEqualityComparer<TKey> equalityComparer)
        {
            // Validate parameters.
            if (outer == null) throw new ArgumentNullException(nameof(outer));
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Get a lookup.
            ILookup<TKey, TInner> innerLookup = inner
                .ToLookup(innerKeySelector, equalityComparer);

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
                    if (innerLookup.Contains(key))
                    {
                        // Yield the entire sequence for the left.
                        foreach (TInner innerItem in innerLookup[key])
                            yield return (outerItem, innerItem);

                        // Add the item to the matched keys.
                        matchedKeys.Add(key);
                    }
                    else
                        // Yield just the outer item with the default.
                        yield return (outerItem, unmatchedInner);
                }

                // The remainder.
                // Everything in the inner lookup keys that
                // has not been matched.
                IEnumerable<TInner> remainingItems = innerLookup
                    .Where(g => !matchedKeys.Contains(g.Key))
                    .SelectMany(g => innerLookup[g.Key]);

                // Yield.
                foreach (TInner remainingItem in remainingItems)
                    yield return (unmatchedOuter, remainingItem);
            }

            // Return.
            return Implementation();
        }
    }
}