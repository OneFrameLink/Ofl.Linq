using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        public static async Task<int> CountAsync<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken = default
        )
        {
            // Validate paramweters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // The count.
            int count = 0;

            // Cycle.
            await foreach (T t in source.WithCancellation(cancellationToken))
                ++count;

            // Return the count.
            return count;
        }
    }
}
