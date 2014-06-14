using Hans.FileSystem;
using Hans.General;
using Hans.Services;
using System.IO;

namespace Hans.Web
{
    public class DownloadRequest
    {
        public string DestinationDirectory { get; set; }

        public IDownloader Downloader { get; set; }

        public string FileName { get; set; }

        public FileSystem.FileSystem FileSystem { get; set; }

        public IOnlineServiceTrack OnlineServiceTrack { get; set; }

        public string ServiceName { get; set; }

        public string Uri { get; set; }

        private string GetSafeFileName()
        {
            return FileName.RemoveIllegalCharacters();
        }
    }
}