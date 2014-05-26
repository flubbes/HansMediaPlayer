using System;
using NAudio.Wave;

namespace Hans.Library
{
    public class HansSong
    {
        private AudioFileReader _fileReader;
        public HansSong(string path)
        {
            Path = path;
        }

        public long Length
        {
            get { return 0; }
        }

        public long Position
        {
            get { return 0; }
        }

        public String Path { get; set; }

        public void PrepareToPlay()
        {
            var val = new AudioFileReader("path").Length;
        }
    }
}