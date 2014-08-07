using System;
using Hans.Core.Database.Songs;

namespace Hans.Core.Audio
{
    /// <summary>
    /// The event arguments when a song started playing
    /// </summary>
    public class StartedPlayingEventArgs : EventArgs
    {
        /// <summary>
        /// The song that started playing
        /// </summary>
        public HansSong Song { get; set; }
    }
}