using System;

namespace Hans.SongData
{
    /// <summary>
    /// A find song data request
    /// </summary>
    public class FindSongDataRequest
    {
        /// <summary>
        /// The path to the file
        /// </summary>
        public string PathToFile { get; set; }

        /// <summary>
        /// The song id
        /// </summary>
        public Guid SongId { get; set; }
    }
}