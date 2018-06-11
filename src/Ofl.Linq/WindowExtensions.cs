using System.Collections.Generic;
using System.Linq;

namespace Ofl.Linq
{
    public static class WindowExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this in Window<T> window) =>
            window.Behind.Append(window.Item).Concat(window.Ahead);
    }
}
