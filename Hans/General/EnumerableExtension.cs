using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hans.General
{
    public static class EnumerableExtension
    {
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