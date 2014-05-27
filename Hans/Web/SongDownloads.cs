using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Bson;

namespace Hans.Web
{
    public class SongDownloads
    {
        private readonly List<DownloadRequest> _activeDownloads;
        public event DownloadFinishedEventHandler DownloadFinished;

        public SongDownloads()
        {
            _activeDownloads = new List<DownloadRequest>();
            new Thread(DownloadProgressCheckerMethod)
            {
                IsBackground = true
            }.Start();
        }

        private void DownloadProgressCheckerMethod()
        {
            while (true)
            {
                CheckProgress();
                Thread.Sleep(50);
            }
        }

        private void CheckProgress()
        {
            for (var i = 0; i < _activeDownloads.Count; i++)
            {
                CheckProgress(i);
            }
        }

        private void CheckProgress(int i)
        {
            var request = _activeDownloads[i];
            if (request.Downloader.Progress != 100)
            {
                return;
            }
            OnDownloadFinished(request);
            _activeDownloads.RemoveAt(i);
        }

        private void OnDownloadFinished(DownloadRequest request)
        {
            if (DownloadFinished != null)
            {
                DownloadFinished(this, new DownloadFinishedEventHandlerArgs(request));
            }
        }

        public void Start(DownloadRequest downloadRequest)
        {
            _activeDownloads.Add(downloadRequest);
            downloadRequest.Downloader.Start(downloadRequest);
        }

        public IEnumerable<DownloadRequest> ActiveDownloads
        {
            get { return _activeDownloads; }
        }
    }
}
