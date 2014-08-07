using System;
using System.Collections.Generic;
using System.Linq;

namespace Hans.Components.Services.LinkCrawl
{
    /// <summary>
    /// Extension methods for enumerables to implement distinct by
    /// </summary>
    public static class Compare
    {
        /// <summary>
        /// Equality comparer
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TIdentity"></typeparam>
        /// <param name="identitySelector"></param>
        /// <returns></returns>
        public static IEqualityComparer<TSource> By<TSource, TIdentity>(Func<TSource, TIdentity> identitySelector)
        {
            return new DelegateComparer<TSource, TIdentity>(identitySelector);
        }

        /// <summary>
        /// Distincts by a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIdentity"></typeparam>
        /// <param name="source"></param>
        /// <param name="identitySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, TIdentity>(this IEnumerable<T> source, Func<T, TIdentity> identitySelector)
        {
            return source.Distinct(Compare.By(identitySelector));
        }

        /// <summary>
        /// The delegate comparer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TIdentity"></typeparam>
        private class DelegateComparer<T, TIdentity> : IEqualityComparer<T>
        {
            private readonly Func<T, TIdentity> identitySelector;

            public DelegateComparer(Func<T, TIdentity> identitySelector)
            {
                this.identitySelector = identitySelector;
            }

            public bool Equals(T x, T y)
            {
                return Equals(identitySelector(x), identitySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return identitySelector(obj).GetHashCode();
            }
        }
    }
}