using System.Collections.Generic;

namespace Hans.SongData
{
    public interface IDataFindMethod
    {
        Dictionary<string, string> GetData(FindSongDataRequest request);
    }
}