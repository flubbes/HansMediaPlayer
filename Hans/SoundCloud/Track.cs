using System;

namespace Hans.SoundCloud
{
    public class Track
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Mp3Url { get; set; }

        public string DisplayName
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        public Track(string artist, string title, string mp3Url)
        {
            Artist = artist;
            Title = title;
            Mp3Url = mp3Url;
        }

        public String GetFileName()
        {
            var fileName = DisplayName + ".mp3";
            return fileName.ToAllowedFileName();
        }
    }
}