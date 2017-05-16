using System;
using System.Collections;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public class EnumerableProxy<T> : IEnumerable<T>
    {
        #region Constructor

        public EnumerableProxy(IEnumerable<T> enumerable)
        {
            // Validate parameters.
            _enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
        }

        #endregion

        #region Private, reado-only state.

        private readonly IEnumerable<T> _enumerable;

        #endregion

        #region IEnumerable<T> implementation.

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator() => _enumerable.GetEnumerator();

        #endregion
    }
}
