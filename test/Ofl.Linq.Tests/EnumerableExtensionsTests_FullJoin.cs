using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ofl.Linq.Tests
{
    // ReSharper disable InconsistentNaming
    public class EnumerableExtensionsTests_FullJoin
    // ReSharper restore InconsistentNaming
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void Test_No_Matching_Items(int count)
        {
            // Create the sequences.
            IEnumerable<int?> outer = Enumerable.Range(0, count).AsNullables();
            IEnumerable<int?> inner = Enumerable.Range(count, count).AsNullables();

            // Join.
            IReadOnlyCollection<(int? outer, int? inner)> joined = outer
                .FullJoin(inner, o => o, i => i)
                .ToReadOnlyCollection();

            // The count should be double.
            Assert.Equal(count * 2, joined.Count);

            // No items should have matched.
            Assert.False(joined.Any(p => p.inner == p.outer), "There are items that matched between sequences.");            
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void Test_All_Items_Match(int count)
        {
            // Create the sequences.
            IEnumerable<int?> outer = Enumerable.Range(0, count).AsNullables();
            IEnumerable<int?> inner = Enumerable.Range(0, count).AsNullables();

            // Join.
            IReadOnlyCollection<(int? outer, int? inner)> joined = outer
                .FullJoin(inner, o => o, i => i)
                .ToReadOnlyCollection();

            // The count is equal to the count.
            Assert.Equal(count, joined.Count);

            // All items should have matched.
            Assert.True(joined.All(p => p.inner == p.outer), "There are items that did not match between sequences.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(100)]
        public void Test_All_Items_Match_Cartesian_Product(int count)
        {
            // Create the sequences.
            IEnumerable<int?> outer = Enumerable.Repeat(0, count).AsNullables();
            IEnumerable<int?> inner = Enumerable.Repeat(0, count).AsNullables();

            // Join.
            IReadOnlyCollection<(int? outer, int? inner)> joined = outer
                .FullJoin(inner, o => o, i => i)
                .ToReadOnlyCollection();

            // The count is the count squared.
            Assert.Equal(count * count, joined.Count);

            // All items should have matched.
            Assert.True(joined.All(p => p.inner == p.outer), "There are items that did not match between sequences.");
        }

        [Theory]
        // No need to test 0 or 1 because we want some to match, some not to match.
        [InlineData(2)]
        [InlineData(100)]
        public void Test_Some_Items_Match(int count)
        {
            // Create the sequences.  Only want one overlapping item.
            IEnumerable<int?> outer = Enumerable.Range(0, count).AsNullables();
            IEnumerable<int?> inner = Enumerable.Range(count - 1, count).AsNullables();

            // Join.
            IReadOnlyCollection<(int? outer, int? inner)> joined = outer
                .FullJoin(inner, o => o, i => i)
                .ToReadOnlyCollection();

            // Count should be count * 2 - 1
            Assert.Equal(count * 2 - 1, joined.Count);

            // The number of outer and inner items that are null should be count - 1.
            Assert.Equal(count - 1, joined.Count(p => p.outer is null));
            Assert.Equal(count - 1, joined.Count(p => p.inner is null));

            // One item should have matched.
            Assert.Equal(1, joined.Count(p => p.outer == p.inner));

        }

        [Theory]
        // Do not test 0, as we rely on count - 1 and don't want to deal with
        // the negative (this is tested anyways in no matching items test).
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(100)]
        public void Test_Some_Items_Match_Multiple(int count)
        {
            // The amount that matches and doesn't match.
            int matches = count / 2;
            int diffs = count - matches;

            // Create the sequences.  Only want one overlapping item.
            IEnumerable<int?> outer = Enumerable.Repeat(0, matches).Concat(Enumerable.Range(1, diffs)).AsNullables();
            IEnumerable<int?> inner = Enumerable.Repeat(0, matches).Concat(Enumerable.Range(count, diffs)).AsNullables();

            // Join.
            IReadOnlyCollection<(int? outer, int? inner)> joined = outer
                .FullJoin(inner, o => o, i => i)
                .ToReadOnlyCollection();

            // Count is matches ^ 2 + diffs * 2
            Assert.Equal(Math.Pow(matches, 2) + diffs * 2, joined.Count);

            // The number of diffs on each side should be diffs.
            Assert.Equal(diffs, joined.Count(p => p.inner is null));
            Assert.Equal(diffs, joined.Count(p => p.outer is null));

            // One item should have matched.
            Assert.Equal(Math.Pow(matches, 2), joined.Count(p => p.outer == p.inner));
        }
    }
}
