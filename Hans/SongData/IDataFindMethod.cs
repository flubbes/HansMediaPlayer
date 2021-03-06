﻿using System.Collections.Generic;

namespace Hans.SongData
{
    /// <summary>
    /// Represents a data find method
    /// </summary>
    public interface IDataFindMethod
    {
        Dictionary<string, string> GetData(FindSongDataRequest request);
    }
}