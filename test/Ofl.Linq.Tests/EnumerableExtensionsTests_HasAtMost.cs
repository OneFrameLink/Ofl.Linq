using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ofl.Linq.Tests
{
    // ReSharper disable InconsistentNaming
    public class EnumerableExtensionsTests_HasAtMost
    // ReSharper restore InconsistentNaming
    {
        #region Helpers.

        private static void AssertHasAtMost<T>(IEnumerable<T> sequence, int count, bool expectException)
        {
            // Validate parameters.
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count,
                $"The {nameof(count)} parameter must be a non-negative value.");

            // Was there an exception?
            Exception? exception = null;

            // Wrap in a try catch.
            try
            {
                // Drain.
                sequence.HasAtMost(count).Drain();
            }
            catch (Exception e)
            {
                // Set the exception.
                exception = e;
            }

            // If not expecting an exception, and there was, throw.
            if (!expectException && exception != null)
                throw new InvalidOperationException("An exception was thrown when an exception was not expected", exception);

            // If expecting an exception and none was thrown, throw.
            if (expectException && exception == null)
                throw new InvalidOperationException("An exception was expected but none was thrown.");
        }

        #endregion

        #region Tests.

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 1, false)]
        [InlineData(1, 0, true)]
        [InlineData(1, 1, false)]
        [InlineData(100, 10, true)]
        [InlineData(10, 100, false)]
        public void Test_HasAtMost_Collection(int sequenceLength, int count, bool expectException)
        {
            // Create the sequence.
            var sequence = Enumerable.Range(0, sequenceLength).ToList();

            // Assert the call.
            AssertHasAtMost(sequence, count, expectException);
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 1, false)]
        [InlineData(1, 0, true)]
        [InlineData(1, 1, false)]
        [InlineData(100, 10, true)]
        [InlineData(10, 100, false)]
        public void Test_HasAtMost_ReadOnlyCollection(int sequenceLength, int count, bool expectException)
        {
            // Create the sequence.
            IEnumerable<int> sequence = Enumerable.Range(0, sequenceLength)
                .ToReadOnlyCollection();

            // Assert the call.
            AssertHasAtMost(sequence, count, expectException);
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(0, 1, false)]
        [InlineData(1, 0, true)]
        [InlineData(1, 1, false)]
        [InlineData(100, 10, true)]
        [InlineData(10, 100, false)]
        public void Test_HasAtMost_Enumerable(int sequenceLength, int count, bool expectException)
        {
            // Create the sequence.
            var sequence = Enumerable.Range(0, sequenceLength);

            // Assert the call.
            AssertHasAtMost(sequence, count, expectException);
        }

        #endregion
    }
}
