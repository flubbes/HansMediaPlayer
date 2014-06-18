using Hans.General;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using YoutubeExtractor;

namespace Hans.Web
{
    /// <summary>
    /// The youtube downloader class
    /// </summary>
    public class YouTubeDownloader : IDownloader
    {
        private AudioDownloader _audioDownloader;

        /// <summary>
        /// When the download failed
        /// </summary>
        public event DownloadFailedEventHandler Failed;

        /// <summary>
        /// If the download is complete
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// If currently downlading
        /// </summary>
        public bool IsDownloading { get; private set; }

        /// <summary>
        /// The progress of the download
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Aborts the download CURRENTLY NOT WORKING
        /// </summary>
        public void Abort()
        {
            // TODO find a way to abort the youtube downloader
        }

        /// <summary>
        /// Starts a new download
        /// </summary>
        /// <param name="request"></param>
        public void Start(DownloadRequest request)
        {
            IsDownloading = true;
            InitialzeAudioDownloader(request, GetVideoInfo(request));
            HookEvents();
            try
            {
                _audioDownloader.Execute();
            }
            catch
            {
                Debug.WriteLine("Song not available");
                OnFailed(request);
            }
        }

        /// <summary>
        /// Triggers the fail event
        /// </summary>
        /// <param name="request"></param>
        protected virtual void OnFailed(DownloadRequest request)
        {
            var handler = Failed;
            if (handler != null)
            {
                handler(this, new DownloadFailedEventArgs { Request = request });
            }
            IsDownloading = false;
        }

        /// <summary>
        /// Gets the first video with extractable audio
        /// </summary>
        /// <param name="videoInfos"></param>
        /// <returns></returns>
        private static VideoInfo GetFirstVideoWithExtractableAudio(IEnumerable<VideoInfo> videoInfos)
        {
            return videoInfos.Where(info => info.CanExtractAudio).OrderByDescending(info => info.AudioBitrate).First();
        }

        /// <summary>
        /// Gets the video info from a request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static VideoInfo GetVideoInfo(DownloadRequest request)
        {
            var videoInfos = DownloadUrlResolver.GetDownloadUrls(request.Uri);
            return GetFirstVideoWithExtractableAudio(videoInfos);
        }

        /// <summary>
        /// Gets called when the audio extraction progress changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void _audioExtractionProgressChanged(object sender, ProgressEventArgs args)
        {
            Progress = Convert.ToInt32(85 + args.ProgressPercentage * 0.15);
        }

        /// <summary>
        /// Gets called when the download finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _downloadFinished(object sender, EventArgs e)
        {
            Progress = 100;
            IsDownloading = false;
            IsComplete = true;
        }

        /// <summary>
        /// Gest called when the download progress changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void _downloadProgressChanged(object sender, ProgressEventArgs args)
        {
            Progress = Convert.ToInt32(args.ProgressPercentage * 0.85);
        }

        /// <summary>
        /// Hooks all events
        /// </summary>
        private void HookEvents()
        {
            _audioDownloader.DownloadProgressChanged += _downloadProgressChanged;
            _audioDownloader.AudioExtractionProgressChanged += _audioExtractionProgressChanged;
            _audioDownloader.DownloadFinished += _downloadFinished;
        }

        /// <summary>
        /// Initializes the audio downloader
        /// </summary>
        /// <param name="request"></param>
        /// <param name="video"></param>
        private void InitialzeAudioDownloader(DownloadRequest request, VideoInfo video)
        {
            _audioDownloader = new AudioDownloader(video,
                Path.Combine(request.DestinationDirectory, request.OnlineServiceTrack.DisplayName.RemoveIllegalCharacters() + video.AudioExtension));
        }
    }
}