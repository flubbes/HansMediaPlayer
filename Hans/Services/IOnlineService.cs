using System.Collections.Generic;
using Hans.Tests;

namespace Hans.Services
{
    public interface IOnlineService
    {
        IEnumerable<IOnlineServiceTrack> Search(string query);
    }
}