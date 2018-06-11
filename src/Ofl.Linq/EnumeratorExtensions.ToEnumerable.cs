using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static class EnumeratorExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            // The implementation.
            IEnumerable<T> Implementation() {
                // Move next, yield the current.
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }

            // Call the implementation.
            return Implementation();
        }

        public static IEnumerable<T> ToEnumerableAfter<T>(this IEnumerator<T> enumerator)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            // The implementation.
            IEnumerable<T> Implementation() {
                // Yield first
                do
                {
                    // Yield.
                    yield return enumerator.Current;
                    // While there are items.
                } while (enumerator.MoveNext());
            }

            // Call the implementation.
            return Implementation();
        }
    }
}
