using System;
using System.Collections.Generic;
using Hans.Core.Database.Songs;

namespace Hans.Components.Library
{
    /// <summary>
    /// The library search finished event arguments
    /// </summary>
    public class LibrarySearchFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// The tracks that got found
        /// </summary>
        public IEnumerable<HansSong> Tracks { get; set; }
    }
}