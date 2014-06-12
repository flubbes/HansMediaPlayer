using Hans.Database.Songs;
using Hans.Library;

namespace Hans.SongData
{
    public interface ISongDataFinder
    {
        event FoundDataEventHandler FoundData;
        void FindAsync(FindSongDataRequest request);
    }
}