using System;
using System.Security.Cryptography.X509Certificates;

namespace Hans.Web
{
    public interface IDownloader
    {
        event EventHandler Failed;

        bool IsComplete { get; }

        bool IsDownloading { get; }

        int Progress { get; }

        void Abort();

        void Start(DownloadRequest request);
    }
}