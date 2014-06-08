using System;

namespace Hans.Library
{
    public struct SongDataResponse
    {
        public Guid SongId;
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
    }
}