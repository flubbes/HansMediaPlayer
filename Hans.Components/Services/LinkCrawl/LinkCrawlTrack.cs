using Hans.Components.Web;
using Hans.Core.Services;
using Hans.Core.Web;

namespace Hans.Components.Services.LinkCrawl
{
    /// <summary>
    /// The link crawl online servcie track
    /// </summary>
    public class LinkCrawlTrack : IOnlineServiceTrack
    {
        /// <summary>
        /// The artist
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The wonderful display name
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// The mp3 url
        /// </summary>
        public string Mp3Url { get; set; }

        /// <summary>
        /// The name of the service
        /// </summary>
        public string ServiceName { get; private set; }

        /// <summary>
        /// The title of the track
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Creates a downloader object for this track
        /// </summary>
        /// <returns></returns>
        public IDownloader GetDownloader()
        {
            return new HttpDownloader();
        }

        /// <summary>
        /// Gets the filename
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return string.Format("{0} - {1}.mp3", Artist, Title);
        }
    }
}