using Hans.Web;
using Newtonsoft.Json;

namespace Hans.Services.YouTube
{
    /// <summary>
    /// The youtube track
    /// </summary>
    internal class YouTubeTrack : IOnlineServiceTrack
    {
        /// <summary>
        /// The artist of the track
        /// </summary>
        public string Artist
        {
            get { return Snippet.ChannelTitle; }
            set { Snippet.ChannelTitle = value; }
        }

        /// <summary>
        /// THe display name
        /// </summary>
        public string DisplayName
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        /// <summary>
        /// The fileextension
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// The mp3Url
        /// </summary>
        public string Mp3Url
        {
            get { return "https://www.youtube.com/watch?v=" + Id.VideoId; }
            set { Id.VideoId = value; }
        }

        /// <summary>
        /// The service name
        /// </summary>
        public string ServiceName
        {
            get { return "Youtube"; }
        }

        /// <summary>
        /// The title of the track
        /// </summary>
        [JsonProperty("title")]
        public string Title
        {
            get { return Snippet.Title; }
            set { Snippet.Title = value; }
        }

        /// <summary>
        /// The id of the song
        /// </summary>
        [JsonProperty("id")]
        private YouTubeId Id { get; set; }

        /// <summary>
        /// The snippet of the song
        /// </summary>
        [JsonProperty("snippet")]
        private YouTubeSnippet Snippet { get; set; }

        /// <summary>
        /// Gets the downloader
        /// </summary>
        /// <returns></returns>
        public IDownloader GetDownloader()
        {
            return new YouTubeDownloader();
        }

        /// <summary>
        /// Gets the filenames
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return string.Format("{0} - {1}{2}", Artist, Title, ".mp3");
        }
    }
}