using Hans.Core.Database.Songs;
using NAudio.Wave;

namespace Hans.Core.Audio
{
    /// <summary>
    /// The audio loader interface
    /// </summary>
    public interface IAudioLoader
    {
        /// <summary>
        /// Prepares the audio file to play
        /// </summary>

        AudioFileReader Load(HansSong song);

        /// <summary>
        /// The method that gets called at the end to wipe all data within the player
        /// </summary>
        void UnLoad();
    }
}