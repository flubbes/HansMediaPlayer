using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;
using Hans.General;
using Hans.Properties;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Hans.Web
{
    public class SongDownloads
    {
        private readonly List<DownloadRequest> _activeDownloads;
        private readonly object _lock = new object();
        private bool _exit;

        public SongDownloads(ExitAppTrigger exitAppTrigger)
        {
            exitAppTrigger.GotTriggered += exitAppTrigger_GotTriggered;
            _activeDownloads = new List<DownloadRequest>();
            new Thread(DownloadProgressCheckerMethod)
            {
                IsBackground = true
            }.Start();
            ParalelDownloads = 1;
        }

        public event DownloadFinishedEventHandler DownloadFinished;

        public IEnumerable<DownloadRequest> ActiveDownloads
        {
            get
            {
                return _activeDownloads.BuildThreadSafeCopy();
            }
        }

        public int ParalelDownloads { get; set; }

        public void Start(DownloadRequest downloadRequest)
        {
            CreateTempDirectoryIfNotExists();
            _activeDownloads.Add(downloadRequest);
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
            lock (_lock)
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

        private int CountActiveDownloads()
        {
            return _activeDownloads.Count(d => d.Downloader.IsDownloading);
        }

        private void DownloadProgressCheckerMethod()
        {
            while (!_exit)
            {
                lock (_lock)
                {
                    while (_activeDownloads.Any(d => !d.Downloader.IsDownloading) && CountActiveDownloads() < ParalelDownloads)
                    {
                        var request = _activeDownloads.FirstOrDefault(d => !d.Downloader.IsDownloading);
                        if (request != null)
                        {
                            request.Downloader.Start(request);
                        }
                    }
                }
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