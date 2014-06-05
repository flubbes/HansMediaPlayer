using System;
using System.Collections.Generic;

namespace Hans.Library
{
    public class HansPlaylist
    {
        public Guid Id;

        public IEnumerable<HansSong> Songs { get; set; }

        public string Name { get; set; }
    }
}