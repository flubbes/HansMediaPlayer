using Hans.General;
using Hans.Web;
using Newtonsoft.Json;
using System;

namespace Hans.Services.SoundCloud
{
    /// <summary>
    /// THe sound cloud service track
    /// </summary>
    public class SoundCloudTrack : IOnlineServiceTrack
    {
        private string _downloadUrl;
        private string _streamUrl;

        /// <summary>
        /// The artist
        /// </summary>
        public string Artist
        {
            get { return User.Username; }
            set { User.Username = value; }
        }

        /// <summary>
        /// The display name
        /// </summary>
        public string DisplayName
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        /// <summary>
        /// If the stream is downloadable
        /// </summary>
        [JsonProperty("downloadable")]
        public bool Downloadable { get; set; }

        /// <summary>
        /// The url to download
        /// </summary>
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

        /// <summary>
        /// The genre
        /// </summary>
        [JsonProperty("genre")]
        public string Genre { get; set; }

        /// <summary>
        /// The id
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The mp3 url
        /// </summary>
        public string Mp3Url { get; set; }

        /// <summary>
        /// The service name
        /// </summary>
        public string ServiceName
        {
            get { return "SoundCloud"; }
        }

        /// <summary>
        /// If the song is streamable
        /// </summary>
        [JsonProperty("streamable")]
        public bool Streamable { get; set; }

        /// <summary>
        /// The streamurl
        /// </summary>
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

        /// <summary>
        /// The title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The user who downloaded the song
        /// </summary>
        [JsonProperty("user")]
        public SoundCloudUser User { get; set; }

        /// <summary>
        /// Gets the downloader
        /// </summary>
        /// <returns></returns>
        public IDownloader GetDownloader()
        {
            return new HttpDownloader();
        }

        /// <summary>
        /// The filename
        /// </summary>
        /// <returns></returns>
        public String GetFileName()
        {
            var fileName = string.Format("{0} - {1}.mp3", Artist, Title);
            return fileName.RemoveIllegalCharacters();
        }
    }
}