using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken = default
        )
        {
            // Validate paramweters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Create the list.
            var list = new List<T>();

            // Cycle.
            await foreach (T t in source.WithCancellation(cancellationToken))
                // Add to the list.
                list.Add(t);

            // Return the list.
            return list;
        }
    }
}
