using Hans.Tests;
using NAudio.Wave;

namespace Hans.Library
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

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Genre { get; set; }

        public string FilePath { get; set; }
    }
}