using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hans.Tests;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Hans.Services.YouTube
{
    public class YouTube : IOnlineService
    {
        private const string ApiKey = "AIzaSyBhQP9f3xhqzR0Q6CpSsJaSd0_brMNOWAg";

        public IEnumerable<IOnlineServiceTrack> Search(string query)
        {
            var restClient = new RestClient("https://www.googleapis.com/youtube/v3/");
            var response = restClient.Execute(new RestRequest("search?q=" + query + "&key=" + ApiKey + "&part=snippet"));
            var a = JArray.Parse(response.Content);
            return a.Select(val => val.ToObject<YouTubeTrack>());
        }
    }
}
