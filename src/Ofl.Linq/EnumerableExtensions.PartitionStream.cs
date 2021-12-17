using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> PartitionStream<T>(
            this IEnumerable<T> source,
            int size
        )
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), size, $"The { nameof(size) } parameter must be a positive value.");

            // Call the internal implementation.
            return source.PartitionStreamImplementation(size);
        }

        private static IEnumerable<IEnumerable<T>> PartitionStreamImplementation<T>(
            this IEnumerable<T> source, int size)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), size, $"The { nameof(size) } parameter must be a positive value.");

            // Get the enumerator.  Dispose of when done.
            using IEnumerator<T> enumerator = source.GetEnumerator();

            do
            {
                // Move to the next element.  If there's nothing left
                // then get out.
                if (!enumerator.MoveNext()) yield break;

                // Return the chunked sequence.
                yield return PartitionStreamSequence(enumerator, size);
            } while (true);
        }

        private static IEnumerable<T> PartitionStreamSequence<T>(IEnumerator<T> enumerator,
            int size)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), size, $"The { nameof(size) } parameter must be a positive value.");

            // The count.
            int count = 0;

            // There is at least one item.  Yield and then continue.
            do
            {
                // Yield the item.
                yield return enumerator.Current;
            } while (++count < size && enumerator.MoveNext());
        }
    }
}
