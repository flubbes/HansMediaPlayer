using System;
using System.Collections.Generic;
using Hans.Database.Songs;

namespace Hans.Database.Playlists
{
    public class HansPlaylist
    {
        public Guid Id;

        public IEnumerable<HansSong> Songs { get; set; }

        public string Name { get; set; }
    }
}