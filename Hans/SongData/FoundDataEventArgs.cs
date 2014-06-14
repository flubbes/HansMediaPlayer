using Hans.Library;
using System;

namespace Hans.SongData
{
    public class FoundDataEventArgs : EventArgs
    {
        public SongDataResponse SongData { get; set; }
    }
}