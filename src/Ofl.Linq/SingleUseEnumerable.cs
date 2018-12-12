using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ofl.Linq
{
    internal class SingleUseEnumerable<T> : IEnumerable<T>
    {
        #region Constructor

        public SingleUseEnumerable(IEnumerator<T> enumerator)
        {
            // Validate parameters.
            _enumerator = enumerator ??
                throw new ArgumentNullException(nameof(enumerator));
        }

        #endregion

        #region Instance state.

        private readonly object _enumeratorLock = new object();

        private IEnumerator<T> _enumerator;

        #endregion

        #region IEnumerable<T> implementation

        public IEnumerator<T> GetEnumerator()
        {
            // Lock on the enumerator lock.
            lock (_enumeratorLock)
            {
                // If the enumerator is null, throw.
                if (_enumerator == null)
                    throw new InvalidOperationException($"{nameof(GetEnumerator)} may only be called once on this instance of {nameof(SingleUseEnumerable<T>)}.");

                // Copy the enumerator.
                var enumeratorCopy = _enumerator;

                // Set to null.
                _enumerator = null;

                // Return.
                return enumeratorCopy;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
