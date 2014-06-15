using Newtonsoft.Json;

namespace Hans.Services.SoundCloud
{
    /// <summary>
    /// A soundcloud user
    /// </summary>
    public class SoundCloudUser
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}