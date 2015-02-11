using System.Collections.Generic;
using System.IO;

namespace Hans.Core.SongData.DataFindMethods
{
    public class FileNameDataFindMethod : IDataFindMethod
    {
        public Dictionary<string, string> GetData(FindSongDataRequest request)
        {
            var fileName = Path.GetFileNameWithoutExtension(request.PathToFile);
            var split = fileName.Contains("-") ? fileName.Split('-') : new string[0];
            if (split.Length == 2)
            {
                return new Dictionary<string, string> {{"artist", split[0]}, {"title", split[1]}};
            }
            return new Dictionary<string, string>();
        }
    }
}
