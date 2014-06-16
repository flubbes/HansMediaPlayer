using Hans.Services;

namespace Hans.General
{
    /// <summary>
    /// The searchrequest data class
    /// </summary>
    public struct SearchRequest
    {
        /// <summary>
        /// The online service to search in
        /// </summary>
        public IOnlineService OnlineService { get; set; }

        /// <summary>
        /// The query to search for
        /// </summary>
        public string Query { get; set; }
    }
}