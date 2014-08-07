using System.Collections.Generic;

namespace Hans.Core.Services
{
    /// <summary>
    /// An online service
    /// </summary>
    public interface IOnlineService
    {
        /// <summary>
        /// The name of the service
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Search the service
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<IOnlineServiceTrack> Search(string query);
    }
}