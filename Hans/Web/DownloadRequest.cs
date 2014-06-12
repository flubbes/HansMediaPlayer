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

        public IOnlineServiceTrack OnlineServiceTrack { get; set; }

        public string ServiceName { get; set; }

        public string Uri { get; set; }

        public string GetAbsolutePath()
        {
            return Path.GetFullPath(GetRelativePath());
        }

        public string GetRelativePath()
        {
            var directory = DestinationDirectory;
            var fileName = GetSafeFileName();
            return Path.Combine(directory, fileName);
        }

        private string GetSafeFileName()
        {
            return FileName.RemoveIllegalCharacters();
        }
    }
}