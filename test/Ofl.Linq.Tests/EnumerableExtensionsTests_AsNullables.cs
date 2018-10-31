using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ofl.Linq.Tests
{
    // ReSharper disable InconsistentNaming
    public class EnumerableExtensionsTests_AsNullables
    // ReSharper restore InconsistentNaming
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void Test_Does_Not_Throw(int count)
        {
            // Generate the sequence.
            IEnumerable<int> sequence = Enumerable.Range(0, count);

            // Cast to nullables.
            IEnumerable<int?> nullables = sequence.AsNullables();

            // Drain.
            nullables.Drain();
        }
    }
}
