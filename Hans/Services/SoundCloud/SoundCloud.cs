using Hans.Web;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace Hans.Services.SoundCloud
{
    /// <summary>
    /// The sound cloud search service
    /// </summary>
    public class SoundCloud : IOnlineService
    {
        public const string ApiKey = "df2fea9bed01f8e2743ef1edb11657f5";

        /// <summary>
        /// The downloade of the service
        /// </summary>
        public IDownloader Downloader { get; set; }

        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name
        {
            get { return "SoundCloud"; }
        }

        /// <summary>
        /// Searches sound cloud
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<IOnlineServiceTrack> Search(string query)
        {
            var restClient = new RestClient("https://api.soundcloud.com");
            var response = restClient.Execute(new RestRequest("tracks.json?client_id=" + ApiKey + "&q=" + query,
                Method.GET));
            var a = JArray.Parse(response.Content);
            return a.Select(val => val.ToObject<SoundCloudTrack>());
        }
    }
}