using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Linq
{
    public static partial class AsyncEnumerableExtensions
    {
        public static async ValueTask<ReadOnlyCollection<T>> ToReadOnlyCollection<T>(
            this IAsyncEnumerable<T> source,
            CancellationToken cancellationToken = default
        )
        {
            // Validate paramweters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Create the list.
            List<T> list = await source
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            // Wrap and return.
            return list.WrapInReadOnlyCollection();
        }
    }
}
