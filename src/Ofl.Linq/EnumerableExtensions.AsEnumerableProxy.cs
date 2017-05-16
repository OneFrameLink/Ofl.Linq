using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static EnumerableProxy<T> AsEnumerableProxy<T>(this IEnumerable<T> enumerable)
        {
            // Validate parameters.
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            // Return the new proxy.
            return new EnumerableProxy<T>(enumerable);
        }
    }
}
