using System;
using System.Collections.Generic;
using Hans.Core.Services;

namespace Hans.Components.General
{
    /// <summary>
    /// The event handler for the search finished event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SearchFinishedEventHandler(object sender, SearchFinishedEventArgs e);

    /// <summary>
    /// The event args for the search finished event
    /// </summary>
    public class SearchFinishedEventArgs : EventArgs
    {
        public IEnumerable<IOnlineServiceTrack> Tracks { get; set; }
    }
}