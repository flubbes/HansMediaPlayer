using Hans.Database.Songs;
using NAudio.Wave;
using System.IO;

namespace Hans.Library
{
    internal class DefaultAudioLoader : IAudioLoader
    {
        public AudioFileReader Load(HansSong song)
        {
            return new AudioFileReader(song.FilePath);
        }

        public void UnLoad()
        {
        }
    }
}