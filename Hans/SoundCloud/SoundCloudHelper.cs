using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hans.SoundCloud
{
    public static class SoundCloudHelper
    {
        /// <summary>
        /// Returns the tracks available at the page with the specified source code.
        /// </summary>
        /// <param name="htmlCode">The HTML code of the page.</param>
        /// <returns>The list of available tracks.</returns>
        public static List<Track> GetTracks(String htmlCode)
        {
            var mp3Urls = GetMediaUrls(htmlCode);
            var titles = GetTitles(htmlCode);
            var artists = GetArtists(htmlCode);

            var tracks = new List<Track>();

            if (mp3Urls.Count != titles.Count 
                || mp3Urls.Count != artists.Count 
                || titles.Count != artists.Count)
            {
                // We won't be able to assign the artist and the title to every urls we have
                // Create tracks with only the url

                tracks.AddRange(mp3Urls.Select(mp3Url => new Track("Unknown", "Unknown", mp3Url)));
            }
            else
            {
                using (var e1 = mp3Urls.GetEnumerator())
                using (var e2 = titles.GetEnumerator())
                using (var e3 = artists.GetEnumerator())
                {
                    while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                    {
                        var mp3Url = e1.Current;
                        var title = e2.Current;
                        var artist = e3.Current;

                        tracks.Add(new Track(artist, title, mp3Url));
                    }
                }
            }

            return tracks;
        }

        private static List<String> GetMediaUrls(String htmlCode)
        {
            // "streamUrl":"http://media.soundcloud.com/stream/blabla"
            var regex = new Regex(@"""streamUrl"":""(?<url>http://media\.soundcloud\.com/stream/[^""]+)""");

            return (from Match match in regex.Matches(htmlCode) select match.Groups["url"].Value).ToList();
        }

        private static List<String> GetTitles(String htmlCode)
        {
            // "title":"blabla"
            var regex = new Regex(@"""title"":""(?<title>[^""]+)""");

            return (from Match match in regex.Matches(htmlCode) select ConvertUnicodeStrings(match.Groups["title"].Value)).ToList();
        }

        private static List<String> GetArtists(String htmlCode)
        {
            // "username":"blabla"
            var regex = new Regex(@"""username"":""(?<artist>[^""]*)""");

            return (from Match match in regex.Matches(htmlCode) select ConvertUnicodeStrings(match.Groups["artist"].Value)).ToList();
        }

        private static String ConvertUnicodeStrings(String str)
        {
            // Unicode characters appear as "_uXXXX" in SoundClound html source.
            // For instance, we have "_u00e9" for 'é'
            // We will replace these "_uXXXX" occurences by the real character
            return Regex.Replace(str, @"\\u([0-9A-Fa-f]{4})",
                m => ((char) Convert.ToInt32(m.Groups[1].Value, 16)).ToString(CultureInfo.InvariantCulture));
        }
    }
}
