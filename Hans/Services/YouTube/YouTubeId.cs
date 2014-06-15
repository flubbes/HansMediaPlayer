using Newtonsoft.Json;

namespace Hans.Services.YouTube
{
    /// <summary>
    /// A youtube id
    /// </summary>
    public class YouTubeId
    {
        /// <summary>
        /// The kind of this youtube id (type) YOU are kind
        /// </summary>
        [JsonProperty("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// The video id
        /// </summary>
        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }
}