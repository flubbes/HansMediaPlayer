using Hans.Core.Audio;
using Hans.Core.Database.Songs;
using NAudio.Wave;

namespace Hans.Components.Audio
{
    /// <summary>
    /// The default audio loader
    /// </summary>
    public class DefaultAudioLoader : IAudioLoader
    {
        /// <summary>
        /// Loads a song
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public AudioFileReader Load(HansSong song)
        {
            return new AudioFileReader(song.FilePath);
        }

        /// <summary>
        /// Unloads a song
        /// </summary>
        public void UnLoad()
        {
        }
    }
}