using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Safeguard<T>(this IEnumerable<T> source)
        {
            // If null, return null, otherwise, return the source.
            return source ?? Enumerable.Empty<T>();
        }

        private static class SafeguardImplementation<T>
        {
            internal static readonly ReadOnlyCollection<T> Empty = new ReadOnlyCollection<T>(new T[0]);
        }

        public static IReadOnlyCollection<T> Safeguard<T>(this IReadOnlyCollection<T> source)
        {
            return source ?? SafeguardImplementation<T>.Empty;
        }

        public static IReadOnlyList<T> Safeguard<T>(this IReadOnlyList<T> source)
        {
            return source ?? SafeguardImplementation<T>.Empty;
        }
    }
}
