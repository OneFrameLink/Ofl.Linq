using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<TValue> source, Func<TValue, TKey> keySelector
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            // Create the dictionary and wrap.
            return source.ToDictionary(keySelector).WrapInReadOnlyDictionary();
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<TValue> source, Func<TValue, TKey> keySelector,
            IEqualityComparer<TKey> comparer
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Create the dictionary and wrap.
            return source.ToDictionary(keySelector, comparer).WrapInReadOnlyDictionary();
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            Func<TSource, TValue> elementSelector,
            IEqualityComparer<TKey> comparer
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Create the dictionary and wrap.
            return source.ToDictionary(keySelector, elementSelector, comparer).WrapInReadOnlyDictionary();
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            Func<TSource, TValue> elementSelector
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null) throw new ArgumentNullException(nameof(elementSelector));

            // Create the dictionary and wrap.
            return source.ToDictionary(keySelector, elementSelector).WrapInReadOnlyDictionary();
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Call the overload.
            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(
                p => p.Key, p => p.Value, EqualityComparer<TKey>.Default));
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // Create and return.
            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(
                p => p.Key, p => p.Value, comparer));
        }
    }
}
