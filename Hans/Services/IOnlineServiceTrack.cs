using Hans.Web;

namespace Hans.Services
{
    /// <summary>
    /// An online service track
    /// </summary>
    public interface IOnlineServiceTrack
    {
        /// <summary>
        /// The artist
        /// </summary>
        string Artist { get; set; }

        /// <summary>
        /// The display name
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The mp3 url
        /// </summary>
        string Mp3Url { get; set; }

        /// <summary>
        /// The serivce name
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// The title of the track
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets the downloader of the track
        /// </summary>
        /// <returns></returns>
        IDownloader GetDownloader();

        /// <summary>
        /// Gets the file name
        /// </summary>
        /// <returns></returns>
        string GetFileName();
    }
}