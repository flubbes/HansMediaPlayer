using Hans.Database.Songs;
using NAudio.Wave;

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
