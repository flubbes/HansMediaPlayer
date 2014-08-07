using System;

namespace Hans.Core.Database.PlaylistSongs
{
    /// <summary>
    /// A song from a playlist in hans
    /// </summary>
    public class PlaylistSong
    {
        /// <summary>
        /// The id of the playlist
        /// </summary>
        public Guid PlaylistId { get; set; }

        /// <summary>
        /// The song id
        /// </summary>
        public Guid SongId { get; set; }
    }
}