namespace Hans.Core.SongData
{
    /// <summary>
    /// The song data finder interface
    /// </summary>
    public interface ISongDataFinder
    {
        /// <summary>
        /// When it found new song data
        /// </summary>
        event FoundDataEventHandler FoundData;

        /// <summary>
        /// Starts the song data finder
        /// </summary>
        /// <param name="request"></param>
        void FindAsync(FindSongDataRequest request);
    }
}