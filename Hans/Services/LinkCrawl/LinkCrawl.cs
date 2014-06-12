using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Services.LinkCrawl
{
    public static class Compare
    {
        public static IEqualityComparer<TSource> By<TSource, TIdentity>(Func<TSource, TIdentity> identitySelector)
        {
            return new DelegateComparer<TSource, TIdentity>(identitySelector);
        }

        public static IEnumerable<T> DistinctBy<T, TIdentity>(this IEnumerable<T> source, Func<T, TIdentity> identitySelector)
        {
            return source.Distinct(Compare.By(identitySelector));
        }

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

    internal class LinkCrawl : IOnlineService
    {
        public string Name { get; private set; }

        public IEnumerable<IOnlineServiceTrack> Search(string query)
        {
            if (IsUri(query))
            {
                var uri = GetUri(query);
                CQ cq = new WebClient().DownloadString(uri);
                foreach (var obj in GetLinks(cq))
                {
                    var link = obj.GetAttribute("href");
                    yield return new LinkCrawlTrack
                    {
                        Mp3Url = link,
                        Artist = Path.GetFileNameWithoutExtension(link)
                    };
                }
            }
        }

        private static IEnumerable<IDomObject> GetLinks(CQ cq)
        {
            return cq["a"].Where(o => o.GetAttribute("href") != null && o.GetAttribute("href").EndsWith(".mp3")).DistinctBy(a => a.GetAttribute("href"));
        }

        private static Uri GetUri(string query)
        {
            return new Uri(query);
        }

        private bool IsUri(string query)
        {
            try
            {
                new Uri(query);
                return true;
            }
            catch { }
            return false;
        }
    }
}