using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static void Drain<T>(this IEnumerable<T> source)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Get the enumerator.
            using (IEnumerator<T> enumerator = source.GetEnumerator())
                // Drain.
                while (enumerator.MoveNext())
                    ;
        }
    }
}
