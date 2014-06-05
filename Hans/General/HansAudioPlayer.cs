using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Hans.Database;
using Hans.Database.Songs;
using Hans.Library;
using Hans.Properties;
using Hans.Services;
using Hans.Web;
using NAudio.Wave;

namespace Hans.General
{
    public class HansAudioPlayer
    {
        private volatile int _listPosition;
        private readonly SongDownloads _songDownloads;
        private volatile List<HansSong> _songQueue;

        public HansAudioPlayer(HansMusicLibrary library)
        {
            Library = library;
            _listPosition = 0;
            _songQueue = new List<HansSong>();
            Player = new WaveOut();
            _songDownloads = new SongDownloads();
            _songDownloads.DownloadFinished += _songDownloads_DownloadFinished;
        }

        public event SearchFinishedEventHandler SearchFinished;

        public event SongQueueChangedEventHandler SongQueueChanged;

        public HansSong CurrentlyPlaying
        {
            get
            {
                return IsQueueEmpty() ? null : _songQueue.ElementAt(_listPosition);
            }
        }

        public HansMusicLibrary Library { get; set; } 

        public bool IsPlaying
        {
            get { return Player.PlaybackState == PlaybackState.Playing; }
        }

        private IWavePlayer Player { get; set; }

        public bool Reapeat { get; set; }

        public bool Shuffle { get; set; }

        public IEnumerable<HansSong> SongQueue
        {
            get
            {
                lock (_songQueue)
                {
                    return _songQueue;
                }
            }
        }

        public float Volume { get; set; }

        public void Download(IOnlineServiceTrack track)
        {
            // Async
            // mp3???
            // Catch 403
            // TODO iDownloader hasFailed
            _songDownloads.Start(new DownloadRequest
            {
                DestinationPath = Settings.Default.Download_Temp_Directory,
                Downloader = new YouTubeDownloader(),
                OnlineServiceTrack = track,
                Uri = track.Mp3Url
            });

            /*_songDownloads.Start(new DownloadRequest
            {
                DestinationPath = track.DisplayName + ".mp3",
                Downloader = new HttpDownloader(),
                OnlineServiceTrack = track,
                Uri = track.Mp3Url
            });*/
        }

        public void Next()
        {

        }

        public void Pause()
        {

        }

        public void Play()
        {
            if (IsQueueEmpty())
            {
                return;
            }
            //TODO implement playing functionality
            //_songQueue[_listPosition].PrepareToPlay(new RamAudioLoader());
            //Player.Init(_songQueue[_listPosition].WaveStream);
            Player.Play();
        }

        public void Previous()
        {

        }

        public void Search(SearchRequest searchRequest)
        {
            new Thread(() => StartSearch(searchRequest))
            {
                IsBackground = true
            }.Start();
        }

        public void Stop()
        {

        }

        private void _songDownloads_DownloadFinished(object sender, DownloadFinishedEventHandlerArgs args)
        {
            _songQueue.Add(
                    HansSong.FromOnlineServiceTrack(
                            args.DownloadRequest.DestinationPath,
                            args.DownloadRequest.OnlineServiceTrack
                        )
                );
            OnSongQueueChanged();
        }

        private bool IsQueueEmpty()
        {
            return !_songQueue.Any();
        }

        private void OnSearchFinished(IEnumerable<IOnlineServiceTrack> tracks)
        {
            if (SearchFinished != null)
            {
                SearchFinished(tracks);
            }
        }

        private void OnSongQueueChanged()
        {
            if (SongQueueChanged != null)
            {
                SongQueueChanged(this, EventArgs.Empty);
            }
        }
        private void StartSearch(SearchRequest searchRequest)
        {
            OnSearchFinished(searchRequest.OnlineService.Search(searchRequest.Query));
        }
    }
}
