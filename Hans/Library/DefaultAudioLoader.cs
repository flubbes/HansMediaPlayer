using Hans.Database.Songs;
using NAudio.Wave;
using System.IO;

namespace Hans.Library
{
    /// <summary>
    /// The default audio loader
    /// </summary>
    internal class DefaultAudioLoader : IAudioLoader
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