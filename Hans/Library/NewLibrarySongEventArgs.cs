using Hans.Database.Songs;
using System;

namespace Hans.Library
{
    /// <summary>
    /// The new library song event args
    /// </summary>
    public class NewLibrarySongEventArgs : EventArgs
    {
        /// <summary>
        /// The new library song
        /// </summary>
        public HansSong Song { get; set; }
    }
}