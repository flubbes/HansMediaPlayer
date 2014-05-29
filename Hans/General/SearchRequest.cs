using Hans.Services;

namespace Hans.General
{
    public struct SearchRequest
    {
        public string Query { get; set; }
        public IOnlineService OnlineService { get; set; }
    }
}