namespace Hans.Models
{
    /// <summary>
    /// The downloader list view item model
    /// </summary>
    public class DownloaderListViewItem
    {
        /// <summary>
        /// The artist
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// The progress
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// The service name
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// The title
        /// </summary>
        public string Title { get; set; }
    }
}