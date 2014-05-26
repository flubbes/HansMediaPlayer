namespace Hans.Web
{
    public interface IDownloader
    {
        void Start(DownloadRequest request);
        int Progress { get; }
        void Abort();
    }
}
