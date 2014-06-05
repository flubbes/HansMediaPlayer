using System;
using Hans.Services;

namespace Hans.Database.Songs
{
    public class HansSong
    {
        public HansSong(string filePath)
        {
            FilePath = filePath;
        }

        public static HansSong FromOnlineServiceTrack(string path, IOnlineServiceTrack track)
        {
            return new HansSong(path)
            {
                Artist = track.Artist,
                Title = track.Title
            };
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Genre { get; set; }

        public string FilePath { get; set; }
    }
}