using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> HasAtMost<T>(this IEnumerable<T> source, int count)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count,
                $"The {nameof(count)} parameter must be a non-negative value.");

            // Exception factory.
            InvalidOperationException CreateException() =>
                new InvalidOperationException($"A call to {nameof(HasAtMost)} was made with an enumeration that has more than {count} elements.");

            // Sniff.
            if (source is ICollection<T> c)
                // Throw or return.
                return c.Count > count ? throw CreateException() : source;

            // Check read only collection.
            if (source is IReadOnlyCollection<T> roc)
                // Throw or return.
                return roc.Count > count ? throw CreateException() : source;

            // Default implementation for IEnumerable.
            IEnumerable<T> Implementation() {
                // How many were yielded.
                int yielded = 0;

                // Grab the enumerator.
                using (IEnumerator<T> enumerator = source.GetEnumerator())
                {
                    // Loop the read.
                    while (enumerator.MoveNext())
                    {
                        // Increment yielded.
                        yielded++;

                        // If greater than count, throw.
                        if (yielded > count)
                            // Throw.
                            throw CreateException();

                        // Yield the item.
                        yield return enumerator.Current;
                    }
                }
            }

            // Return the implementation.
            return Implementation();
        }
    }
}
