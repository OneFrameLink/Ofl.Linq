using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<Window<T>> LookAhead<T>(this IEnumerable<T> source, int ahead)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (ahead < 0) throw new ArgumentOutOfRangeException(nameof(ahead), 0, $"The {nameof(ahead)} parameter must be a non-negative number.");

            // Just call window.
            return source.Window(0, ahead);
        }
    }
}
