using Newtonsoft.Json;

namespace Hans.Components.Services.YouTube
{
    /// <summary>
    /// A youtube snippet
    /// </summary>
    public class YouTubeSnippet
    {
        /// <summary>
        /// The channel title
        /// </summary>
        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }

        /// <summary>
        /// published at datetime
        /// </summary>
        [JsonProperty("publishedAt")]
        public string PublishDate { get; set; }

        /// <summary>
        /// The title of the snippet
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}