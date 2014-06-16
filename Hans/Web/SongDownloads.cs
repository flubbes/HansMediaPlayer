using Hans.General;
using Hans.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Hans.Web
{
    /// <summary>
    /// The song downloader
    /// </summary>
    public class SongDownloads
    {
        private readonly List<DownloadRequest> _activeDownloads;
        private readonly object _lock = new object();
        private bool _exit;

        /// <summary>
        /// Initializes a new instance of the song downloader
        /// </summary>
        /// <param name="exitAppTrigger"></param>
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

        /// <summary>
        /// When a download is finished
        /// </summary>
        public event DownloadFinishedEventHandler DownloadFinished;

        /// <summary>
        /// Gets the active and pending downloads
        /// </summary>
        public IEnumerable<DownloadRequest> ActiveDownloads
        {
            get
            {
                return _activeDownloads.BuildThreadSafeCopy();
            }
        }

        /// <summary>
        /// The maximum allowed downloads
        /// </summary>
        public int ParalelDownloads { get; set; }

        /// <summary>
        /// Starts a new download
        /// </summary>
        /// <param name="downloadRequest"></param>
        public void Start(DownloadRequest downloadRequest)
        {
            CreateTempDirectoryIfNotExists();
            _activeDownloads.Add(downloadRequest);
        }

        /// <summary>
        /// Creates the temp directory if necessary
        /// </summary>
        private static void CreateTempDirectoryIfNotExists()
        {
            var downloadTempDirectory = Settings.Default.Download_Temp_Directory;
            if (!Directory.Exists(downloadTempDirectory))
            {
                Directory.CreateDirectory(downloadTempDirectory);
            }
        }

        /// <summary>
        /// Counts the active downloads
        /// </summary>
        /// <returns></returns>
        private int CountActiveDownloads()
        {
            return _activeDownloads.Count(d => d.Downloader.IsDownloading);
        }

        /// <summary>
        /// The download progress checker thread method
        /// </summary>
        private void DownloadProgressCheckerMethod()
        {
            while (!_exit)
            {
                RemoveFinishedDownloads();
                StartDownloadsIfNecessary();
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Gets called when the app exits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void exitAppTrigger_GotTriggered(object sender, EventArgs eventArgs)
        {
            _exit = true;
        }

        /// <summary>
        /// Triggers the download finished event
        /// </summary>
        /// <param name="request"></param>
        private void OnDownloadFinished(DownloadRequest request)
        {
            if (DownloadFinished != null)
            {
                DownloadFinished(this, new DownloadFinishedEventArgs { DownloadRequest = request });
            }
        }

        /// <summary>
        /// Removes all finished downloads
        /// </summary>
        private void RemoveFinishedDownloads()
        {
            lock (_lock)
            {
                for (var i = 0; i < _activeDownloads.Count; i++)
                {
                    RemoveIfDownloadFinished(i);
                }
            }
        }

        /// <summary>
        /// Removes an index from the download queue if necessary
        /// </summary>
        /// <param name="i"></param>
        private void RemoveIfDownloadFinished(int i)
        {
            var request = _activeDownloads[i];
            if (request.Downloader.Progress != 100)
            {
                return;
            }
            OnDownloadFinished(request);
            _activeDownloads.RemoveAt(i);
        }

        /// <summary>
        /// Starts downloads if necessary
        /// </summary>
        private void StartDownloadsIfNecessary()
        {
            lock (_lock)
            {
                while (_activeDownloads.Any(d => !d.Downloader.IsDownloading && !d.Downloader.IsComplete) && CountActiveDownloads() < ParalelDownloads)
                {
                    var request = _activeDownloads.FirstOrDefault(d => !d.Downloader.IsDownloading && !d.Downloader.IsComplete);
                    if (request != null)
                    {
                        request.Downloader.Start(request);
                    }
                }
            }
        }
    }
}