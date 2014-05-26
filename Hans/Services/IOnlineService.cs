using System.Collections.Generic;

namespace Hans.Tests
{
    public interface IOnlineService
    {
        IEnumerable<IOnlineServiceTrack> Search(string query);
    }
}