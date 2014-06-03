﻿using System.IO;
using NAudio.Wave;
using RestSharp.Extensions;

namespace Hans.Library
{
    class RamAudioLoader : IAudioLoader
    {
        public WaveStream Load(HansSong song)
        {
            return new AudioFileReader(song.FilePath);
        }

        public void UnLoad()
        {
            
        }
    }
}