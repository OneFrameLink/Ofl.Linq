using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            CancellationToken cancellationToken = default
        )
        where TKey : notnull =>
            source.ToDictionaryAsync(
                keySelector,
                s => s,
                cancellationToken
            );

        public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> elementSelector,
            CancellationToken cancellationToken = default
        )
        where TKey : notnull =>
            source.ToDictionaryAsync(
                keySelector, 
                elementSelector, 
                EqualityComparer<TKey>.Default, 
                cancellationToken
            );

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer,
            CancellationToken cancellationToken = default
        )
        where TKey : notnull
            => source.ToDictionaryAsync(keySelector, s => s, comparer, cancellationToken);

        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(
            this IAsyncEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> elementSelector,
            IEqualityComparer<TKey> comparer,
            CancellationToken cancellationToken = default
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Create the dictionary.
            var dictionary = new Dictionary<TKey, TValue>(comparer);

            // Cycle through the items.
            await foreach (TSource t in source)
            {
                // Check for cancellation.
                cancellationToken.ThrowIfCancellationRequested();

                // Get the key.
                TKey key = keySelector(t);
                TValue value = elementSelector(t);

                // Add.
                dictionary.Add(key, value);
            }

            // Return the value.
            return dictionary;
        }
    }
}
