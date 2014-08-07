using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Hans.Core.General;

namespace Hans.Core.Web
{
    /// <summary>
    /// The song downloader
    /// </summary>
    public class SongDownloads
    {
        private readonly ThreadSafeList<DownloadRequest> _activeDownloads;
        private bool _exit;
        private Thread _thread;

        /// <summary>
        /// Initializes a new instance of the song downloader
        /// </summary>
        /// <param name="exitAppTrigger"></param>
        public SongDownloads(ExitAppTrigger exitAppTrigger)
        {
            exitAppTrigger.GotTriggered += exitAppTrigger_GotTriggered;
            _activeDownloads = new ThreadSafeList<DownloadRequest>();
            StartProgressCheckerThread();
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
            get { return _activeDownloads.GetEnumerable(); }
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
            var downloadTempDirectory = StaticSettings.DownloadTempDirectory;
            if (!Directory.Exists(downloadTempDirectory))
            {
                Directory.CreateDirectory(downloadTempDirectory);
            }
        }

        private static bool IsUnstartedDownloader(IDownloader item)
        {
            return !item.IsDownloading && !item.IsComplete;
        }

        /// <summary>
        /// Counts the active downloads
        /// </summary>
        /// <returns></returns>
        private int CountActiveDownloads()
        {
            var result = 0;
            _activeDownloads.ThreadSafeForeach(r =>
            {
                result += r.Downloader.IsDownloading ? 1 : 0;
                return false;
            });
            return result;
        }

        /// <summary>
        /// Gets called when the download operation failed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloaderOnFailed(object sender, DownloadFailedEventArgs e)
        {
            _activeDownloads.Remove(e.Request);
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
                Thread.Sleep(100);
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
        /// Gets the first unstarted download from the activeDownloads list
        /// </summary>
        /// <returns>The fist unstarted download</returns>
        private DownloadRequest GetFirstUnstartedDownload()
        {
            var res = default(DownloadRequest);
            _activeDownloads.ThreadSafeForeach(r =>
            {
                if (IsUnstartedDownloader(r.Downloader))
                {
                    res = r;
                    return true;
                }
                return false;
            });
            return res;
        }

        /// <summary>
        /// Determines whether the download queue has unstarted downloads
        /// </summary>
        /// <returns></returns>
        private bool HasUnstartedDownloads()
        {
            var result = false;
            _activeDownloads.ThreadSafeForeach(r =>
            {
                if (IsUnstartedDownloader(r.Downloader))
                {
                    result = true;
                    return true;
                }
                return false;
            });
            return result;
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
            for (var i = 0; i < _activeDownloads.Count; i++)
            {
                RemoveIfDownloadFinished(i);
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

        private bool ShouldStartNewDownloads()
        {
            return CountActiveDownloads() < ParalelDownloads;
        }

        /// <summary>
        /// Starts downloads if necessary
        /// </summary>
        private void StartDownloadsIfNecessary()
        {
            if (HasUnstartedDownloads() && ShouldStartNewDownloads())
            {
                var request = GetFirstUnstartedDownload();
                if (request != null)
                {
                    request.Downloader.Failed += DownloaderOnFailed;
                    request.Downloader.Start(request);
                }
            }
        }

        private void StartProgressCheckerThread()
        {
            _thread = new Thread(DownloadProgressCheckerMethod);
            _thread.Start();
        }
    }
}