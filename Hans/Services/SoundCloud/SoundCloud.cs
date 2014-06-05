using System.Collections.Generic;
using System.Linq;
using Hans.Web;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Hans.Services.SoundCloud
{
    public class SoundCloud : IOnlineService
    {
        public const string ApiKey = "df2fea9bed01f8e2743ef1edb11657f5";
        
        public IEnumerable<IOnlineServiceTrack> Search(string query)
        {
            var restClient = new RestClient("https://api.soundcloud.com");
            var response = restClient.Execute(new RestRequest("tracks.json?client_id=" + ApiKey + "&q=" + query,
                Method.GET));
            var a = JArray.Parse(response.Content);
            return a.Select(val => val.ToObject<SoundCloudTrack>());
        }

        public string Name
        {
            get { return "SoundCloud"; }
        }
    }
}
