using System;
using Hans.Tests;
using Newtonsoft.Json;

namespace Hans.Services.YouTube
{
    internal class YouTubeTrack : IOnlineServiceTrack
    {
        [JsonProperty("id")]
        private YouTubeId Id { get; set; }

        [JsonProperty("snippet")]
        private YouTubeSnippet Snippet { get; set; }

        public string Mp3Url
        {
            get { return "https://www.youtube.com/watch?v=" + Id.VideoId; }
            set { Id.VideoId = value; }
        }

        public string Artist
        {
            get { return Snippet.ChannelTitle; }
            set { Snippet.ChannelTitle = value; }
        }

        [JsonProperty("title")]
        public string Title
        {
            get { return Snippet.Title; }
            set { Snippet.Title = value; }
        }

        public string DisplayName
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        public string GetFileName()
        {
            throw new NotImplementedException();
        }
    }

    public class YouTubeId
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }

    public class YouTubeSnippet
    {
        [JsonProperty("publishedAt")]
        public string PublishDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }
    }
}
