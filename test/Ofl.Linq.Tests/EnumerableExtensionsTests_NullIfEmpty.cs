using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ofl.Linq.Tests
{
    // ReSharper disable InconsistentNaming
    public class EnumerableExtensionsTests_NullIfEmpty
    // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void Test_Null_If_Empty()
        {
            // Create the sequences.
            IEnumerable<int>? empty = Enumerable.Empty<int>();

            // Filter.
            empty = empty.NullIfEmpty();

            // It should be null.
            Assert.Null(empty);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Test_Not_Empty_Is_Not_Null(int count)
        {
            // Validate parameters.
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), count,
                $"The {nameof(count)} parameter must be a positive value.");

            // Create the sequence.
            IEnumerable<int>? sequence = Enumerable.Range(0, count);

            // Filter.
            sequence = sequence.NullIfEmpty();

            // It should not be null.
            Assert.NotNull(sequence);

            // The sequences are equal.
            Assert.True(sequence.SequenceEqual(Enumerable.Range(0, count)));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void Test_Not_Empty_Is_Not_Null_And_Resettable(int count)
        {
            // Validate parameters.
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), count,
                $"The {nameof(count)} parameter must be a positive value.");

            // Create the sequence.  Need to materialize so it's supported.
            IEnumerable<int>? sequence = Enumerable.Range(0, count).ToList();

            // Filter.
            sequence = sequence.NullIfEmpty();

            // It should not be null.
            Assert.NotNull(sequence);

            // Get the enumerator.
            using IEnumerator<int> enumerator = sequence!.GetEnumerator();

            // First time, this will skip.
            Assert.True(Enumerable.Range(0, count).SequenceEqual(enumerator.ToEnumerable()));

            // Reset.
            enumerator.Reset();

            // This will not skip, but it will not matter.
            Assert.True(Enumerable.Range(0, count).SequenceEqual(enumerator.ToEnumerable()));
        }
    }
}
