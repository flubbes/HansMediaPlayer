using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Hans.Library
{
    public struct SongDataResponse
    {
        public List<string> Artists { get; set; }

        public string FilePath { get; set; }

        public string Genre { get; set; }

        public Guid SongId { get; set; }

        public string Title { get; set; }
    }
}