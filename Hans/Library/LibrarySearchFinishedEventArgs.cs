using Hans.Database.Songs;
using System;
using System.Collections.Generic;

namespace Hans.Library
{
    public class LibrarySearchFinishedEventArgs : EventArgs
    {
        public IEnumerable<HansSong> Tracks { get; set; }
    }
}