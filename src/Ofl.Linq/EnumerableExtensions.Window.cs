using System;
using System.Collections.Generic;

namespace Ofl.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<Window<T>> Window<T>(this IEnumerable<T> source, int behind, int ahead)
        {
            // Validate parameters.
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (behind < 0) throw new ArgumentOutOfRangeException(nameof(behind), 0, $"The {nameof(behind)} parameter must be a non-negative number.");
            if (ahead < 0) throw new ArgumentOutOfRangeException(nameof(ahead), 0, $"The {nameof(ahead)} parameter must be a non-negative number.");

            // The implementation.
            IEnumerable<Window<T>> Implementation() {
                // Get the enumerator.
                using var enumerator = source.GetEnumerator();

                // The behind and ahead lists.
                var behindList = new List<T>(behind);
                var aheadList = new List<T>(ahead);

                // The wrappers.
                IReadOnlyList<T> behindWrapper = behindList.WrapInReadOnlyCollection();
                IReadOnlyList<T> afterWrapper = aheadList.WrapInReadOnlyCollection();

                // Updates the queues.
                Window<T> UpdateBeforeYield()
                {
                    // Pop.
                    T current = aheadList[0];

                    // Remove the item (if there are items)
                    aheadList.RemoveAt(0);

                    // If there are more items in before than the count, remove
                    // the first from the before list.
                    if (behindList.Count > behind)
                        // Remove.
                        behindList.RemoveAt(0);

                    // The window, needs to be set here so
                    // that we have the proper references.
                    return new Window<T>(current, behindWrapper, afterWrapper);
                }

                // Updates after.
                void UpdateAfterYield(T item) => behindList.Add(item);

                // While there are items to move to.
                while (enumerator.MoveNext())
                {
                    // Queue on the after list.
                    aheadList.Add(enumerator.Current);

                    // If the length of the after list is greater than the after amount, pop, yield, then move to before.
                    if (aheadList.Count <= ahead) continue;

                    // Get the window.
                    Window<T> window = UpdateBeforeYield();

                    // Yield the window.
                    yield return window;

                    // Update after.
                    UpdateAfterYield(window.Item);
                }

                // Drain the after list.
                while (aheadList.Count > 0)
                {
                    // Get the window.
                    Window<T> window = UpdateBeforeYield();

                    // Yield the window.
                    yield return window;

                    // Update after.
                    UpdateAfterYield(window.Item);
                }
            }

            // Return the implementation.
            return Implementation();
        }
    }
}
