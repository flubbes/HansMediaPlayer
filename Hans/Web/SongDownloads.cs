using Hans.General;
using Hans.Properties;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Hans.Web
{
    public class SongDownloads
    {
        private volatile List<DownloadRequest> _activeDownloads;
        private bool _exit;

        public SongDownloads(ExitAppTrigger exitAppTrigger)
        {
            exitAppTrigger.GotTriggered += exitAppTrigger_GotTriggered;
            _activeDownloads = new List<DownloadRequest>();
            new Thread(DownloadProgressCheckerMethod)
            {
                IsBackground = true
            }.Start();
        }

        public event DownloadFinishedEventHandler DownloadFinished;

        public IEnumerable<DownloadRequest> ActiveDownloads
        {
            get
            {
                lock (_activeDownloads)
                {
                    return _activeDownloads.ToEnumerable();
                }
            }
        }

        public void Start(DownloadRequest downloadRequest)
        {
            CreateTempDirectoryIfNotExists();
            lock (_activeDownloads)
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

        private void DownloadProgressCheckerMethod()
        {
            while (!_exit)
            {
                CheckProgress();
                Thread.Sleep(50);
            }
        }

        private void exitAppTrigger_GotTriggered(object sender, EventArgs eventArgs)
        {
            _exit = true;
        }

        private void OnDownloadFinished(DownloadRequest request)
        {
            if (DownloadFinished != null)
            {
                DownloadFinished(this, new DownloadFinishedEventHandlerArgs(request));
            }
        }
    }
}