using System;
using Hans.SoundCloud;
using Hans.Tests;
using Newtonsoft.Json;

namespace Hans.Services.SoundCloud
{
    public class SoundCloudTrack : IOnlineServiceTrack
    {
        private string _streamUrl;
        private string _downloadUrl;

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("downloadable")]
        public bool Downloadable { get; set; }

        [JsonProperty("streamable")]
        public bool Streamable { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }


        public string Artist {
            get { return User.Username; }
            set { User.Username = value; }
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        public string Mp3Url { get; set; }

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


        public string DisplayName
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        [JsonProperty("user")]
        public SoundCloudUser User { get; set; }

        public String GetFileName()
        {
            var fileName = DisplayName + ".mp3";
            return fileName.ToAllowedFileName();
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