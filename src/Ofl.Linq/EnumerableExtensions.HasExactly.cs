using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> HasExactly<T>(this IEnumerable<T> source, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, $"The { nameof(count) } parameter must be a non-negative number.");

            IEnumerable<T> Implementation() {
                // Iterate through the items.
                foreach (var t in source)
                {
                    // Decrement the count.  If less than 0, then throw an exception.
                    if (--count < 0)
                        throw new InvalidOperationException($"The sequence must have exactly {count} elements.");

                    // Yield the item.
                    yield return t;
                }

                // If the count is not 0, throw the same exception.
                if (count != 0)
                    throw new InvalidOperationException($"The sequence must have exactly {count} elements.");
            }

            // Call the implementation.
            return Implementation();
        }
    }
}
