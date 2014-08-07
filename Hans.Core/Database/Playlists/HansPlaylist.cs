using System;

namespace Hans.Core.Database.Playlists
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