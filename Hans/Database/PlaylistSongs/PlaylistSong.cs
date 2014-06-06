using System;

namespace Hans.Database.PlaylistSongs
{
    public class PlaylistSong
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
    }
}
