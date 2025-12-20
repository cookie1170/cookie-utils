using System.Collections.Generic;
using JetBrains.Annotations;

namespace CookieUtils
{
    [PublicAPI]
    public static class EnumeratorExtensions
    {
        /// <summary>
        ///     Converts an <c>IEnumerator</c> to an <c>IEnumerable</c>.
        /// </summary>
        /// <param name="e">An instance of <c>IEnumerator</c>.</param>
        /// <returns>An <c>IEnumerable</c> with the same elements as the input instance.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> e)
        {
            while (e.MoveNext())
                yield return e.Current;
        }
    }
}
