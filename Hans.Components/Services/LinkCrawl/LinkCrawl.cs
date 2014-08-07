using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CsQuery;
using Hans.Core.Services;

namespace Hans.Components.Services.LinkCrawl
{
    /// <summary>
    /// The LinkCrawal online service
    /// </summary>
    public class LinkCrawl : IOnlineService
    {
        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name
        {
            get { return "LinkCrawl"; }
        }

        /// <summary>
        /// Crawls links from the given link
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<IOnlineServiceTrack> Search(string query)
        {
            if (!IsUri(query))
            {
                yield break;
            }
            var uri = GetUri(query);
            CQ cq = new WebClient().DownloadString(uri);
            foreach (var link in GetLinks(cq).Select(obj => obj.GetAttribute("href")))
            {
                yield return new LinkCrawlTrack
                {
                    Mp3Url = link,
                    Artist = Path.GetFileNameWithoutExtension(link)
                };
            }
        }

        /// <summary>
        /// Gets all links from a document
        /// </summary>
        /// <param name="cq"></param>
        /// <returns></returns>
        private static IEnumerable<IDomObject> GetLinks(CQ cq)
        {
            return cq["a"].Where(o => o.GetAttribute("href") != null && o.GetAttribute("href").EndsWith(".mp3")).DistinctBy(a => a.GetAttribute("href"));
        }

        /// <summary>
        /// Gets a uri from a stirng
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static Uri GetUri(string query)
        {
            return new Uri(query);
        }

        /// <summary>
        /// Determines whether a string is a uri or not
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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