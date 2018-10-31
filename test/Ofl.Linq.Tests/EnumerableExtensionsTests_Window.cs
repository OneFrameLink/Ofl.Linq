using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ofl.Linq.Tests
{
    // ReSharper disable InconsistentNaming
    public class EnumerableExtensionsTests_Window
    // ReSharper restore InconsistentNaming
    {
        #region Helpers.

        #endregion

        #region Tests.

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 2)]
        [InlineData(1, 2, 0)]
        [InlineData(1, 2, 2)]
        [InlineData(2, 0, 2)]
        [InlineData(2, 2, 0)]
        [InlineData(2, 2, 2)]
        [InlineData(3, 0, 2)]
        [InlineData(3, 2, 0)]
        [InlineData(3, 2, 2)]
        [InlineData(10, 0, 3)]
        [InlineData(10, 3, 0)]
        [InlineData(10, 3, 3)]
        public void Test_Window(int sequenceLength, int behind, int ahead)
        {
            // Create the sequence.
            var sequence = Enumerable.Range(0, sequenceLength);

			// The index.
            int index = 0;

			// Cycle through the items.
            foreach (Window<int> window in sequence.Window(behind, ahead))
            {
				// Assert that the item matches.
				Assert.Equal(index++, window.Item);

                // Get the numbers before and after.
                IEnumerable<int> behindSequence =
                    Enumerable.Range(Math.Max(window.Item - behind, 0), Math.Min(behind, window.Item));
                IEnumerable<int> aheadSequence =
                    Enumerable.Range(Math.Min(window.Item + 1, sequenceLength - 1), Math.Min(ahead, sequenceLength - window.Item - 1));

                // Compare the sequences.
                Assert.Equal(behindSequence, window.Behind);
                Assert.Equal(aheadSequence, window.Ahead);
            }

            // The index is the length of the sequence.
            Assert.Equal(sequenceLength, index);
        }

        #endregion
    }
}
