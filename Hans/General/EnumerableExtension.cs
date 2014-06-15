using System.Collections.Generic;
using System.Linq;

namespace Hans.General
{
    /// <summary>
    /// Extension methods for the enumerable
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Builds a thread safe copy of the enumerable to prevent the Invalidoperation exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static List<T> BuildThreadSafeCopy<T>(this IEnumerable<T> enumerable)
        {
            var r = new List<T>();
            var ts = enumerable as T[] ?? enumerable.ToArray();
            for (var i = 0; i < ts.Count(); i++)
            {
                r.Add(ts.ElementAt(i));
            }
            return r;
        }
    }
}