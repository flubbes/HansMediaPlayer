using System;
using System.Collections.Generic;

namespace Hans.Core.SongData
{
    /// <summary>
    /// Data class for a song data response
    /// </summary>
    public struct SongDataResponse
    {
        /// <summary>
        /// The artists
        /// </summary>
        public List<string> Artists { get; set; }

        /// <summary>
        /// The filepath for the file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The genre
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// The song id
        /// </summary>
        public Guid SongId { get; set; }

        /// <summary>
        /// The title
        /// </summary>
        public string Title { get; set; }
    }
}