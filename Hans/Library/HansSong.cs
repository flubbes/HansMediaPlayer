using System;
using System.IO;
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

        public string FilePath { get; set; }

        public void PrepareToPlay(IAudioLoader loader)
        {
            _loader = loader;
            _loader.Load(this);
        }
    }
}