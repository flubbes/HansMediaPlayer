using Hans.Web;
using YoutubeExtractor;

namespace Hans.Services
{
    public interface IOnlineServiceTrack
    {
        string Artist { get; set; }

        string DisplayName { get; }

        string Mp3Url { get; set; }

        string ServiceName { get; }

        string Title { get; set; }

        IDownloader GetDownloader();

        string GetFileName();
    }
}