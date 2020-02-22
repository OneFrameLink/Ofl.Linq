using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ofl.Linq.Tests
{
    // ReSharper disable InconsistentNaming
    public class EnumerableExtensionsTests_ToAsyncEnumerable
    // ReSharper restore InconsistentNaming
    {
        #region Helpers

        private static async Task AssertEnumerableAsync<T>(
            IAsyncEnumerable<T> enumerable,
            int expected,
            CancellationToken cancellationToken = default
        )
        {
            // Validate parameters.
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (expected < 0)
                throw new ArgumentOutOfRangeException(nameof(expected), expected,
                    $"The {nameof(expected)} parameter must be a positive value.");

            // Count everything.
            int actual = await enumerable
                .CountAsync(cancellationToken)
                .ConfigureAwait(false);

            // Compare.
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public async Task Test_ToAsyncEnumerable_Enumerable(int count)
        {
            // Generate the enumerable.
            IAsyncEnumerable<int> enumerable = Enumerable
                .Range(0, count)
                .ToAsyncEnumerable();

            // Assert.
            await AssertEnumerableAsync(enumerable, count)
                .ConfigureAwait(false);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public async Task Test_ToAsyncEnumerable_List(int count)
        {
            // Generate the enumerable.
            IAsyncEnumerable<int> enumerable = Enumerable
                .Range(0, count)
                .ToList()
                .ToAsyncEnumerable();

            // Assert.
            await AssertEnumerableAsync(enumerable, count)
                .ConfigureAwait(false);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public async Task Test_ToAsyncEnumerable_ReadOnlyList(int count)
        {
            // Generate the enumerable.
            IAsyncEnumerable<int> enumerable = Enumerable
                .Range(0, count)
                .ToReadOnlyCollection()
                .ToAsyncEnumerable();

            // Assert.
            await AssertEnumerableAsync(enumerable, count)
                .ConfigureAwait(false);
        }

        #endregion
    }
}
