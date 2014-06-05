using System.IO;
using Hans.General;
using Hans.Services;

namespace Hans.Web
{
    public class DownloadRequest
    {
        public string Uri { get; set; }
        public string DestinationPath { get; set; }

        public IDownloader Downloader { get; set; }

        public IOnlineServiceTrack OnlineServiceTrack { get; set; }
    }
}