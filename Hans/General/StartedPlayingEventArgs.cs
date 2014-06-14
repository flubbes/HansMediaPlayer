using System;
using Hans.Database.Songs;

namespace Hans.General
{
    public class StartedPlayingEventArgs : EventArgs
    {
        public HansSong Song { get; set; }
    }
}