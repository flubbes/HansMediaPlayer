using Hans.Database.Songs;
using System;
using System.Collections.Generic;

namespace Hans.Database.Playlists
{
    /// <summary>
    /// A playlist from the hans audio player
    /// </summary>
    public class HansPlaylist
    {
        /// <summary>
        /// The id of the playlist
        /// </summary>
        public Guid Id;

        /// <summary>
        /// The name of the playlist
        /// </summary>
        public string Name { get; set; }
    }
}