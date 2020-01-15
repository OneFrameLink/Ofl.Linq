using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ofl.Linq
{
    public readonly struct Window<T>
    {
        #region Constructor

        public Window(T item, IReadOnlyList<T> before, IReadOnlyList<T> after)
        {
            // Validate parameters.
            Behind = before ?? throw new ArgumentNullException(nameof(before));
            Ahead = after ?? throw new ArgumentNullException(nameof(after));

            // Assign values.
            Item = item;
        }

        #endregion

        #region Instance, read-only properties
        
        public T Item { get; }

        public IReadOnlyList<T> Behind { get; }

        public IReadOnlyList<T> Ahead { get; }

        #endregion
    }
}
