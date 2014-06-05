using System.IO;
using Hans.General;
using Hans.Services;

namespace Hans.Web
{
    public class DownloadRequest
    {
        public string Uri { get; set; }
        public string DestinationDirectory { get; set; }
        public string FileName { get; set; }
        public IDownloader Downloader { get; set; }
        public IOnlineServiceTrack OnlineServiceTrack { get; set; }

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