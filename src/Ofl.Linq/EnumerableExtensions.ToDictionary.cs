using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source
        )
            where TKey : notnull
            => source.ToDictionary(EqualityComparer<TKey>.Default);

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source,
            IEqualityComparer<TKey> equalityComparer
        )
        where TKey : notnull
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

            // Convert.
            return source.ToDictionary(p => p.Key, p => p.Value, equalityComparer);
        }
    }
}
