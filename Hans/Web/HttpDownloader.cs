using System;
using System.IO;
using System.Net;
using Hans.General;

namespace Hans.Web
{
    public class HttpDownloader : IDownloader
    {
        private WebClient _webClient;

        public void Start(DownloadRequest request)
        {
            _webClient = new WebClient();
            _webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            _webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            _webClient.DownloadFileAsync(new Uri(request.Uri), request.DestinationPath);
        }

        void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Progress = 100;
        }

        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }

        public int Progress { get; private set; }

        public void Abort()
        {
            _webClient.CancelAsync();
        }
    }
}
