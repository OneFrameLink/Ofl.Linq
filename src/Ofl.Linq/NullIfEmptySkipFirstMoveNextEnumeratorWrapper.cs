using System;
using System.Collections;
using System.Collections.Generic;

namespace Ofl.Linq
{
    internal class NullIfEmptySkipFirstMoveNextEnumeratorWrapper<T> : IEnumerator<T>
    {
        #region Constructor

        public NullIfEmptySkipFirstMoveNextEnumeratorWrapper(IEnumerator<T> inner)
        {
            // Validate parameters.
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        #endregion

        #region Instance state

        private bool _skipped;

        private readonly IEnumerator<T> _inner;

        #endregion

        #region IEnumerator<T> implementation

        public bool MoveNext()
        {
            // If not skipped, skip and return.
            if (!_skipped)
            {
                // Set to true.
                _skipped = true;

                // Return true.
                return true;
            }

            // Pass through.
            return _inner.MoveNext();
        }

        public void Reset()
        {
            // Reset.
            // NOTE: We do not reset skipped because we
            // already know the call to move next will succeed
            // so there's no reason to check on subsequent calls.
            _inner.Reset();
        }

        public T Current => _inner.Current;

        object? IEnumerator.Current => Current;

        public void Dispose() => _inner.Dispose();

        #endregion
    }
}
