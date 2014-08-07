using System;
using Hans.Core.Database.Songs;

namespace Hans.Components.Library
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