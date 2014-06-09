using Hans.General;
using Hans.Web;
using Newtonsoft.Json;
using System;

namespace Hans.Services.SoundCloud
{
    public class SoundCloudTrack : IOnlineServiceTrack
    {
        private string _downloadUrl;
        private string _streamUrl;

        public string Artist
        {
            get { return User.Username; }
            set { User.Username = value; }
        }

        public string DisplayName
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        [JsonProperty("downloadable")]
        public bool Downloadable { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl
        {
            get { return _downloadUrl; }
            set
            {
                value += "?client_id=" + SoundCloud.ApiKey;
                if (!string.IsNullOrEmpty(value))
                {
                    Mp3Url = value;
                }
                _downloadUrl = value;
            }
        }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        public string Mp3Url { get; set; }

        public string ServiceName
        {
            get { return "SoundCloud"; }
        }

        [JsonProperty("streamable")]
        public bool Streamable { get; set; }

        [JsonProperty("stream_url")]
        public string StreamUrl
        {
            get { return _streamUrl; }
            set
            {
                value += "?client_id=" + SoundCloud.ApiKey;
                if (string.IsNullOrEmpty(Mp3Url) && !string.IsNullOrEmpty(value))
                {
                    Mp3Url = value;
                }
                _streamUrl = value;
            }
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("user")]
        public SoundCloudUser User { get; set; }

        public IDownloader GetDownloader()
        {
            return new HttpDownloader();
        }

        public String GetFileName()
        {
            var fileName = string.Format("{0} - {1}.mp3", Artist, Title);
            return fileName.RemoveIllegalCharacters();
        }
    }

    public class SoundCloudUser
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}