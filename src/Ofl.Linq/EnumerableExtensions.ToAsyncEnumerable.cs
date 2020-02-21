using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        #region Extensions.

        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(
            this IEnumerable<T> source
        )
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));

            // Type sniff.
            // Read only list?
            if (source is IReadOnlyList<T> readOnlyList)
                return new ReadOnlyListAsyncEnumerable<T>(readOnlyList);
            if (source is IList<T> list)
                return new ListAsyncEnumerable<T>(list);

            // Last path.
            return new EnumerableAsyncEnumerable<T>(source);
        }

        #endregion

        #region Helpers

        private struct EnumerableAsyncEnumerable<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
        {
            #region Constructor

            public EnumerableAsyncEnumerable(IEnumerable<T> enumerable)
            {
                // Validate parameters.
                _enumerator = enumerable?.GetEnumerator() ?? throw new ArgumentNullException(nameof(enumerable));
                _cancellationTokenSet = false;
            }

            #endregion

            #region Instance state


            private readonly IEnumerator<T> _enumerator;

            private CancellationToken _cancellationToken;

            private bool _cancellationTokenSet;

            #endregion

            #region IAsyncEnumerator<T> implementation

            public T Current => _enumerator.Current;

            public ValueTask<bool> MoveNextAsync()
            {
                // Check cancellation.
                _cancellationToken.ThrowIfCancellationRequested();

                // Return.
                return new ValueTask<bool>(_enumerator.MoveNext());
            }

            #endregion

            #region IAsyncEnumerable<T> implementation

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                // If the cancellation token is set, throw.
                if (_cancellationTokenSet)
                    throw new InvalidOperationException($"Cannot call {nameof(GetAsyncEnumerator)} multiple times.");

                // Set the cancellation token.
                _cancellationToken = cancellationToken;
                _cancellationTokenSet = true;

                // Return this.
                return this;
            }

            public ValueTask DisposeAsync()
            {
                // Dispose of the enumerator.
                using var _ = _enumerator;

                // Return the value task.
                return new ValueTask();
            }

            #endregion
        }

        private struct ListAsyncEnumerable<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
        {
            #region Constructor

            public ListAsyncEnumerable(IList<T> list)
            {
                // Validate parameters.
                _list = list ?? throw new ArgumentNullException(nameof(list));
                _index = -1;
                _cancellationTokenSet = false;
            }

            #endregion

            #region Instance state

            private int _index;

            private readonly IList<T> _list;

            private CancellationToken _cancellationToken;

            private bool _cancellationTokenSet;

            #endregion

            #region IAsyncEnumerator<T> implementation

            public T Current
            {
                get
                {
                    // Validate index.
                    if (_index == -1)
                        throw new InvalidOperationException($"Cannot get the value of the {nameof(Current)} property before calling {nameof(MoveNextAsync)}.");
                    if (_index >= _list.Count)
                        throw new InvalidOperationException($"Cannot get the value of the {nameof(Current)} property after {nameof(MoveNextAsync)} returns false.");

                    // Return the value.
                    return _list[_index];
                }
            }

            public ValueTask<bool> MoveNextAsync()
            {
                // Check cancellation.
                _cancellationToken.ThrowIfCancellationRequested();

                // Return.
                return new ValueTask<bool>(!(_index == _list.Count || ++_index == _list.Count));
            }

            #endregion

            #region IAsyncEnumerable<T> implementation

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                // If the cancellation token is set, throw.
                if (_cancellationTokenSet)
                    throw new InvalidOperationException($"Cannot call {nameof(GetAsyncEnumerator)} multiple times.");

                // Set the cancellation token.
                _cancellationToken = cancellationToken;
                _cancellationTokenSet = true;

                // Return this.
                return this;
            }

            public ValueTask DisposeAsync() => new ValueTask();

            #endregion
        }

        private struct ReadOnlyListAsyncEnumerable<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
        {
            #region Constructor

            public ReadOnlyListAsyncEnumerable(IReadOnlyList<T> list)
            {
                // Validate parameters.
                _list = list ?? throw new ArgumentNullException(nameof(list));
                _index = -1;
                _cancellationTokenSet = false;
            }

            #endregion

            #region Instance state

            private int _index;

            private readonly IReadOnlyList<T> _list;

            private CancellationToken _cancellationToken;

            private bool _cancellationTokenSet;

            #endregion

            #region IAsyncEnumerator<T> implementation

            public T Current
            {
                get
                {
                    // Validate index.
                    if (_index == -1)
                        throw new InvalidOperationException($"Cannot get the value of the {nameof(Current)} property before calling {nameof(MoveNextAsync)}.");
                    if (_index >= _list.Count)
                        throw new InvalidOperationException($"Cannot get the value of the {nameof(Current)} property after {nameof(MoveNextAsync)} returns false.");

                    // Return the value.
                    return _list[_index];
                }
            }

            public ValueTask<bool> MoveNextAsync()
            {
                // Check cancellation.
                _cancellationToken.ThrowIfCancellationRequested();

                // Return.
                return new ValueTask<bool>(!(_index == _list.Count || ++_index == _list.Count));
            }

            #endregion

            #region IAsyncEnumerable<T> implementation

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                // If the cancellation token is set, throw.
                if (_cancellationTokenSet)
                    throw new InvalidOperationException($"Cannot call {nameof(GetAsyncEnumerator)} multiple times.");

                // Set the cancellation token.
                _cancellationToken = cancellationToken;
                _cancellationTokenSet = true;

                // Return this.
                return this;
            }

            public ValueTask DisposeAsync() => new ValueTask();

            #endregion
        }

        #endregion
    }
}
