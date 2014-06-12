using Hans.Web;

namespace Hans.Services.LinkCrawl
{
    internal class LinkCrawlTrack : IOnlineServiceTrack
    {
        public string Artist { get; set; }

        public string DisplayName { get; private set; }

        public string Mp3Url { get; set; }

        public string ServiceName { get; private set; }

        public string Title { get; set; }

        public IDownloader GetDownloader()
        {
            return new HttpDownloader();
        }

        public string GetFileName()
        {
            return string.Format("{0} - {1}.mp3", Artist, Title);
        }
    }
}