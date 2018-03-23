using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<Window<T>> LookBehind<T>(this IEnumerable<T> source, int behind)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (behind < 0) throw new ArgumentOutOfRangeException(nameof(behind), 0, $"The {nameof(behind)} parameter must be a non-negative number.");

            // Just call window.
            return source.Window(behind, 0);
        }
    }
}
