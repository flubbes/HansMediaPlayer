using Hans.Services;
using System;
using System.Collections.Generic;

namespace Hans.General
{
    public delegate void SearchFinishedEventHandler(object sender, SearchFinishedEventArgs e);

    public class SearchFinishedEventArgs : EventArgs
    {
        public IEnumerable<IOnlineServiceTrack> Tracks { get; set; }
    }
}