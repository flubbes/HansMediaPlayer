using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Hans.General;
using YoutubeExtractor;

namespace Hans.Web
{
    class YouTubeDownloader : IDownloader
    {
        private AudioDownloader _audioDownloader;

        public void Start(DownloadRequest request)
        {
            InitialzeAudioDownloader(request, GetVideoInfo(request));
            HookEvents();
            _audioDownloader.Execute();
        }

        private void InitialzeAudioDownloader(DownloadRequest request, VideoInfo video)
        {
            _audioDownloader = new AudioDownloader(video,
                Path.Combine(request.DestinationPath, request.OnlineServiceTrack.DisplayName.RemoveIllegalCharacters() + video.AudioExtension));
        }

        private static VideoInfo GetVideoInfo(DownloadRequest request)
        {
            var videoInfos = DownloadUrlResolver.GetDownloadUrls(request.Uri);
            return GetFirstVideoWithExtractableAudio(videoInfos);
        }

        private static VideoInfo GetFirstVideoWithExtractableAudio(IEnumerable<VideoInfo> videoInfos)
        {
            return videoInfos.Where(info => info.CanExtractAudio).OrderByDescending(info => info.AudioBitrate).First();
        }

        private void HookEvents()
        {
            _audioDownloader.DownloadProgressChanged += _downloadProgressChanged;
            _audioDownloader.AudioExtractionProgressChanged += _audioExtractionProgressChanged;
            _audioDownloader.DownloadFinished += _downloadFinished;
        }

        private void _downloadFinished(object sender, EventArgs e)
        {
            Progress = 100;
        }

        void _downloadProgressChanged(object sender, ProgressEventArgs args)
        {
            Progress = Convert.ToInt32(args.ProgressPercentage * 0.85);
            Debug.WriteLine(Progress);
        }

        void _audioExtractionProgressChanged(object sender, ProgressEventArgs args)
        {
            Progress = Convert.ToInt32(args.ProgressPercentage * 0.15);
        }

        public int Progress { get; private set; }

        public void Abort()
        {   
            // Stop audioDownloader.Execute(); 
            // Non-existing atm.
        }
    }
}
