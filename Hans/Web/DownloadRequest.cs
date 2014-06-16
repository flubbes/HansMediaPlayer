using Hans.General;
using Hans.Services;

namespace Hans.Web
{
    /// <summary>
    /// The download request data class
    /// </summary>
    public class DownloadRequest
    {
        /// <summary>
        /// The destination directory
        /// </summary>
        public string DestinationDirectory { get; set; }

        /// <summary>
        /// The downloader
        /// </summary>
        public IDownloader Downloader { get; set; }

        /// <summary>
        /// The filename
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The filesystem we are working in
        /// </summary>
        public FileSystem.FileSystem FileSystem { get; set; }

        /// <summary>
        /// The online service track to save
        /// </summary>
        public IOnlineServiceTrack OnlineServiceTrack { get; set; }

        /// <summary>
        /// The service nae
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The uri
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets a safe filename
        /// </summary>
        /// <returns></returns>
        private string GetSafeFileName()
        {
            return FileName.RemoveIllegalCharacters();
        }
    }
}