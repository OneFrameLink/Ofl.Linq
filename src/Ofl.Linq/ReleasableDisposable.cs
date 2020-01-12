using System;

namespace Ofl.Linq
{
    internal class ReleasableDisposable<T> : IDisposable
        where T : class, IDisposable
    {
        #region Constructor

        public ReleasableDisposable(T disposable)
        {
            // Assign values.
            _disposable = disposable;
        }

        #endregion

        #region Instance state.

        private T? _disposable;

        public T Disposable
        {
            get
            {
                return _disposable
                    ?? throw new InvalidOperationException(
                        $"Attempted to access the private {nameof(_disposable)} variable which is null.");
            }
        }

        #endregion

        #region Helpers

        public void Release() => _disposable = null;

        #endregion

        #region Disposable implementation

        public void Dispose()
        {
            // Use the disposable.
            using (_disposable)
            { }
        }

        #endregion
    }
}
