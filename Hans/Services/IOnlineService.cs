using System.Collections.Generic;

namespace Hans.Services
{
    public interface IOnlineService
    {
        IEnumerable<IOnlineServiceTrack> Search(string query);
    }
}