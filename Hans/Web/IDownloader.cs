using System;

namespace Hans.Web
{
    public interface IDownloader
    {
        event EventHandler Failed;

        bool IsDownloading { get; }

        int Progress { get; }

        void Abort();

        void Start(DownloadRequest request);
    }
}