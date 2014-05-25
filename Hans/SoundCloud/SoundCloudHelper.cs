using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Hans.SoundCloud
{
    public class SoundCloudHelper
    {
        private static Regex _jsonRegex = new Regex(@"window.SC.bufferTracks.push\((?<JSON>.*?)\);", RegexOptions.Compiled);

        public void Download(string url)
        {
            var matches = _jsonRegex.Matches(new WebClient().DownloadString(url));
            foreach (Match match in matches)
            {
                var json = match.Groups["JSON"].Value;
                var song = new SoundCloudSong
                {
                    Name = Find("title", json),
                    Username = Find("username", json),
                    StreamUrl = Find("username", json),
                    Url = Find("uri", json)
                };
            }
        }

        public void AnalyzeLink()
        {
            
        }

        private string Find(string what, string where)
        {
            var match = Regex.Match(where, String.Format(@"""{0}"":""(?<Value>[^""]*)"",", what));
            return match.Success
                ? HttpUtility.HtmlDecode
                    (
                        Regex.Replace
                            (
                                match.Groups["Value"].Value, @"\\u(?<Code>[a-fA-F0-9]{4})",
                                m => "" + (char) Convert.ToInt32(m.Groups["Code"].Value, 16
                                    )
                            )
                    )
                : null;
        }
    }
}
