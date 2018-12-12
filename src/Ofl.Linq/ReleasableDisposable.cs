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
            Disposable = disposable;
        }

        #endregion

        #region Instance state.

        public T Disposable { get; private set; }

        #endregion

        #region Helpers

        public void Release() => Disposable = null;

        #endregion

        #region Disposable implementation

        public void Dispose()
        {
            // Use the disposable.
            using (Disposable)
            { }
        }

        #endregion
    }
}
