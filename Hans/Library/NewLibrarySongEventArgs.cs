using Hans.Database.Songs;
using System;

namespace Hans.Library
{
    public class NewLibrarySongEventArgs : EventArgs
    {
        public HansSong Song { get; set; }
    }
}