using Hans.General;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using YoutubeExtractor;

namespace Hans.Web
{
    public class YouTubeDownloader : IDownloader
    {
        private AudioDownloader _audioDownloader;

        public event EventHandler Failed;

        public int Progress { get; private set; }

        public void Abort()
        {
            // TODO find a way to abort the youtube downloader
        }

        public void Start(DownloadRequest request)
        {
            InitialzeAudioDownloader(request, GetVideoInfo(request));
            HookEvents();
            try
            {
                _audioDownloader.Execute();
            }
            catch (WebException e)
            {
                Debug.WriteLine("Song not available");
                OnFailed();
            }
        }

        protected virtual void OnFailed()
        {
            var handler = Failed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private static VideoInfo GetFirstVideoWithExtractableAudio(IEnumerable<VideoInfo> videoInfos)
        {
            return videoInfos.Where(info => info.CanExtractAudio).OrderByDescending(info => info.AudioBitrate).First();
        }

        private static VideoInfo GetVideoInfo(DownloadRequest request)
        {
            var videoInfos = DownloadUrlResolver.GetDownloadUrls(request.Uri);
            return GetFirstVideoWithExtractableAudio(videoInfos);
        }

        private void _audioExtractionProgressChanged(object sender, ProgressEventArgs args)
        {
            Progress = Convert.ToInt32(85 + args.ProgressPercentage * 0.15);
        }

        private void _downloadFinished(object sender, EventArgs e)
        {
            Progress = 100;
        }

        private void _downloadProgressChanged(object sender, ProgressEventArgs args)
        {
            Progress = Convert.ToInt32(args.ProgressPercentage * 0.85);
        }

        private void HookEvents()
        {
            _audioDownloader.DownloadProgressChanged += _downloadProgressChanged;
            _audioDownloader.AudioExtractionProgressChanged += _audioExtractionProgressChanged;
            _audioDownloader.DownloadFinished += _downloadFinished;
        }

        private void InitialzeAudioDownloader(DownloadRequest request, VideoInfo video)
        {
            _audioDownloader = new AudioDownloader(video,
                Path.Combine(request.DestinationDirectory, request.OnlineServiceTrack.DisplayName.RemoveIllegalCharacters() + video.AudioExtension));
        }
    }
}