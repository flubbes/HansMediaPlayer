using System;
using System.Net;

namespace Hans.Web
{
    public class HttpDownloader : IDownloader
    {
        private WebClient _webClient;

        public int Progress { get; private set; }

        public void Abort()
        {
            _webClient.CancelAsync();
        }

        public void Start(DownloadRequest request)
        {
            if (request.Uri != null)
            {
                //TODO request.URI can be null. implement download failed event
                _webClient = new WebClient();
                _webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
                _webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
                var uri = new Uri(request.Uri);
                _webClient.DownloadFileAsync(uri, request.GetRelativePath());
            }
        }

        private void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Progress = 100;
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
        }
    }
}