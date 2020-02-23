using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static ValueTask<ReadOnlyDictionary<TKey, TSource>> ToReadOnlyDictionaryAsync<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            CancellationToken cancellationToken = default
        )
        where TKey : notnull =>
            source.ToReadOnlyDictionaryAsync(
                keySelector,
                s => s,
                cancellationToken
            );

        public static ValueTask<ReadOnlyDictionary<TKey, TValue>> ToReadOnlyDictionaryAsync<TSource, TKey, TValue>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> elementSelector,
            CancellationToken cancellationToken = default
        )
        where TKey : notnull =>
            source.ToReadOnlyDictionaryAsync(
                keySelector, 
                elementSelector, 
                EqualityComparer<TKey>.Default, 
                cancellationToken
            );

        public static ValueTask<ReadOnlyDictionary<TKey, TSource>> ToReadOnlyDictionaryAsync<TSource, TKey>(
            this IAsyncEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer,
            CancellationToken cancellationToken = default
        )
        where TKey : notnull
            => source.ToReadOnlyDictionaryAsync(keySelector, s => s, comparer, cancellationToken);

        public static async ValueTask<ReadOnlyDictionary<TKey, TValue>> ToReadOnlyDictionaryAsync<TSource, TKey, TValue>(
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
            Dictionary<TKey, TValue> dictionary = await source.ToDictionaryAsync(
                keySelector,
                elementSelector,
                comparer,
                cancellationToken
            )
            .ConfigureAwait(false);

            // Wrap and return the value.
            return dictionary.WrapInReadOnlyDictionary();
        }
    }
}
