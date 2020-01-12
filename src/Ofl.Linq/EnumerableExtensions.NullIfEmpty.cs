using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T>? NullIfEmpty<T>(this IEnumerable<T> source)
        {
            // Get the enumerator in a releasable disposable.
            using var disposable = new ReleasableDisposable<IEnumerator<T>>(source.GetEnumerator());

            // Get the enumerator.
            IEnumerator<T> enumerator = disposable.Disposable;

            // Move to the next item.  If there are no elements, then return null.
            if (!enumerator.MoveNext()) return null;

            // Release the disposable.
            disposable.Release();

            // Create an enumerator that skips the first move next.
            var wrapper = new NullIfEmptySkipFirstMoveNextEnumeratorWrapper<T>(enumerator);

            // Wrap in a single use enumerator, return that.
            return new SingleUseEnumerable<T>(wrapper);
        }
    }
}
