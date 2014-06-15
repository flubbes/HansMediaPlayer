using Hans.Library;
using System;

namespace Hans.SongData
{
    /// <summary>
    /// The found data event args
    /// </summary>
    public class FoundDataEventArgs : EventArgs
    {
        /// <summary>
        /// The songdata response
        /// </summary>
        public SongDataResponse SongData { get; set; }
    }
}