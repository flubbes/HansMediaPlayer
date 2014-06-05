using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Hans.Services.YouTube
{
    public class YouTube : IOnlineService
    {
        private const int MaxResults = 50;
        private const string ApiKey = "AIzaSyBhQP9f3xhqzR0Q6CpSsJaSd0_brMNOWAg";

        public IEnumerable<IOnlineServiceTrack> Search(string query)
        {
            var restClient = new RestClient("https://www.googleapis.com/youtube/v3/");
            var response = restClient.Execute(new RestRequest("search?q=" + query + "&key=" + ApiKey + "&part=snippet&type=video&maxResults=" + MaxResults));
            var a = JsonConvert.DeserializeObject<JObject>(response.Content)["items"];
            return a.ToObject<IEnumerable<YouTubeTrack>>();
        }

        public string Name
        {
            get { return "Youtube"; }
        }
    }
}
