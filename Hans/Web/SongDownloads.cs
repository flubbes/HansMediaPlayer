using System.Collections.Generic;
using System.IO;
using System.Threading;
using Hans.Properties;
using Ninject.Infrastructure.Language;

namespace Hans.Web
{
    public class SongDownloads
    {
        private volatile List<DownloadRequest> _activeDownloads;
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
            lock (_activeDownloads)
            {
                for (var i = 0; i < _activeDownloads.Count; i++)
                {
                    CheckProgress(i);
                }
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
            CreateTempDirectoryIfNotExists();
            lock(_activeDownloads)
            {
                _activeDownloads.Add(downloadRequest);
            }
            downloadRequest.Downloader.Start(downloadRequest);
        }

        private static void CreateTempDirectoryIfNotExists()
        {
            var downloadTempDirectory = Settings.Default.Download_Temp_Directory;
            if (!Directory.Exists(downloadTempDirectory))
            {
                Directory.CreateDirectory(downloadTempDirectory);
            }
        }

        public IEnumerable<DownloadRequest> ActiveDownloads
        {
            get
            {
                lock(_activeDownloads)
                {
                    return _activeDownloads.ToEnumerable();
                }
            }
        }
    }
}
