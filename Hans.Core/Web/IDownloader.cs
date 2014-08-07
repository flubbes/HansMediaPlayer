namespace Hans.Core.Web
{
    /// <summary>
    /// The downloader interface
    /// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// When the download failed
        /// </summary>
        event DownloadFailedEventHandler Failed;

        /// <summary>
        /// If the download is finished
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// If is currently downloading
        /// </summary>
        bool IsDownloading { get; }

        /// <summary>
        /// Current progress
        /// </summary>
        int Progress { get; }

        /// <summary>
        /// Aborts the download
        /// </summary>
        void Abort();

        /// <summary>
        /// Starts a download
        /// </summary>
        /// <param name="request"></param>
        void Start(DownloadRequest request);
    }
}