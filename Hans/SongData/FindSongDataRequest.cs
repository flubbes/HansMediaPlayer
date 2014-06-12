using System;

namespace Hans.SongData
{
    public class FindSongDataRequest
    {
        public Guid SongId { get; set; }

        public string PathToFile { get; set; }
    }
}