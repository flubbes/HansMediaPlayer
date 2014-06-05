using Hans.Database.Songs;
using NAudio.Wave;

namespace Hans.Library
{
    class DefaultAudioLoader : IAudioLoader
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
