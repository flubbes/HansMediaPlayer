using System;
using System.IO;
using Hans.Tests;
using NAudio.Wave;

namespace Hans.Library
{
    public class HansSong
    {
        private IAudioLoader _loader;

        public HansSong(string filePath)
        {
            FilePath = filePath;
        }

        public long Length
        {
            get { return 0; }
        }

        public long Position
        {
            get { return 0; }
        }

        public string Title { get; set; }
        public string Artist { get; set; }

        public static HansSong FromOnlineServiceTrack(string path, IOnlineServiceTrack track)
        {
            return new HansSong(path)
            {
                Artist = track.Artist,
                Title = track.Title
            };
        }

        public WaveStream WaveStream { get; set; }

        public string FilePath { get; set; }

        public void PrepareToPlay(IAudioLoader loader)
        {
            _loader = loader;
            WaveStream = _loader.Load(this);
        }
    }
}