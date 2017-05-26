using System;
using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2011-04-03</created>
    /// <summary>Contains extensions for LINQ operations.</summary>
    ///
    //////////////////////////////////////////////////
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Append the item.  Concat from.
            return source.Concat(From(item));
        }

    }
}
