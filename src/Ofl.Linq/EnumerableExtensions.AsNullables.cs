using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T?> AsNullables<T>(this IEnumerable<T> source)
            where T : struct
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Just cast.
            return source.Cast<T?>();
        }
    }
}
