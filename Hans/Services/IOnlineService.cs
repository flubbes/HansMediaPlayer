using System.Collections.Generic;
using System.Windows;
using Hans.Web;

namespace Hans.Services
{
    public interface IOnlineService
    {
        IEnumerable<IOnlineServiceTrack> Search(string query);

        string Name { get; }
    }
}