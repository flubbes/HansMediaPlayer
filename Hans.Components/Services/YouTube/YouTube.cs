using System.Collections.Generic;
using Hans.Core.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Hans.Components.Services.YouTube
{
    /// <summary>
    /// The youtube service
    /// </summary>
    public class YouTube : IOnlineService
    {
        private const string ApiKey = "AIzaSyBhQP9f3xhqzR0Q6CpSsJaSd0_brMNOWAg";
        private const int MaxResults = 50;

        /// <summary>
        /// The name if the service
        /// </summary>
        public string Name
        {
            get { return "Youtube"; }
        }

        /// <summary>
        /// Searches on youtube
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<IOnlineServiceTrack> Search(string query)
        {
            var restClient = new RestClient("https://www.googleapis.com/youtube/v3/");
            var response = restClient.Execute(new RestRequest("search?q=" + query + "&key=" + ApiKey + "&part=snippet&type=video&maxResults=" + MaxResults));
            var a = JsonConvert.DeserializeObject<JObject>(response.Content)["items"];
            return a.ToObject<IEnumerable<YouTubeTrack>>();
        }
    }
}