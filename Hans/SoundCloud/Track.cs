using System;

namespace Hans.SoundCloud
{
    public class Track
    {
        public String Artist { get; set; }
        public String Title { get; set; }
        public String Mp3Url { get; set; }
        public String ArtistTitle
        {
            get
            {
                return Artist + " - " + Title;
            }
        }

        public Track(String artist, String title, String mp3Url)
        {
            Artist = artist;
            Title = title;
            Mp3Url = mp3Url;
        }

        public String GetFileName()
        {
            String fileName = ArtistTitle + ".mp3";
            return fileName.ToAllowedFileName();
        }
    }
}