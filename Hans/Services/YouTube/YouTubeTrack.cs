using Hans.Web;
using Newtonsoft.Json;
using System;

namespace Hans.Services.YouTube
{
    public class YouTubeId
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }

    public class YouTubeSnippet
    {
        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonProperty("publishedAt")]
        public string PublishDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    internal class YouTubeTrack : IOnlineServiceTrack
    {
        public string Artist
        {
            get { return Snippet.ChannelTitle; }
            set { Snippet.ChannelTitle = value; }
        }

        public string DisplayName
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        public string FileExtension { get; set; }

        public string Mp3Url
        {
            get { return "https://www.youtube.com/watch?v=" + Id.VideoId; }
            set { Id.VideoId = value; }
        }

        public string ServiceName
        {
            get { return "Youtube"; }
        }

        [JsonProperty("title")]
        public string Title
        {
            get { return Snippet.Title; }
            set { Snippet.Title = value; }
        }

        [JsonProperty("id")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private YouTubeId Id { get; set; }

        [JsonProperty("snippet")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private YouTubeSnippet Snippet { get; set; }

        public IDownloader GetDownloader()
        {
            return new YouTubeDownloader();
        }

        public string GetFileName()
        {
            return string.Format("{0} - {1}{2}", Artist, Title, FileExtension);
        }
    }
}