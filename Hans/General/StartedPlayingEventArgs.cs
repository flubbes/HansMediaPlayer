using Hans.Database.Songs;
using System;

namespace Hans.General
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