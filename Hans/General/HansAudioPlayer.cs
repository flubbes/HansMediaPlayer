using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private readonly IAudioLoader _audioLoader;
        private volatile int _listPosition;
        private readonly SongDownloads _songDownloads;
        private volatile List<HansSong> _songQueue;
        private AudioFileReader _audioFileReader;

        public delegate void NewSongEventHandler();

        public event NewSongEventHandler NewSong;

        public HansAudioPlayer(HansMusicLibrary library, IAudioLoader audioLoader)
        {
            _audioLoader = audioLoader;
            Library = library;
            _listPosition = 0;
            _songQueue = new List<HansSong>();
            Player = new WaveOut();
            Player.PlaybackStopped += Player_PlaybackStopped;
            _songDownloads = new SongDownloads();
            _songDownloads.DownloadFinished += _songDownloads_DownloadFinished;
        }

        void Player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            //TODO refactor
            if (IsQueueEmpty())
            {
                return;
            }
            if (Shuffle)
            {
                _listPosition = new Random().Next(0, _songQueue.Count);
            }
            else
            {
                if (_listPosition == _songQueue.Count)
                {
                    if (!Repeat)
                    {
                        return;
                    }
                    _listPosition = 0;
                }
                else
                {
                    _listPosition++;
                }
            }
            Play();
        }

        protected virtual void OnNewSong()
        {
            var handler = NewSong;
            if (handler != null)
            {
                handler();
            }
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

        public bool Repeat { get; set; }

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

        public float Volume
        {
            get
            {
                return _audioFileReader != null ? _audioFileReader.Volume : 1.0f;
            }
            set
            {
                if (_audioFileReader == null)
                {
                    return;
                }
                _audioFileReader.Volume = value;
            }
        }

        public long CurrentProgress 
        {
            get
            {
                return _audioFileReader != null ? _audioFileReader.Position*100/_audioFileReader.Length : 0;
            }
        }

        public double CurrentSongLength
        {
            get { return _audioFileReader.Length; }
        }

        public long CurrentSongPosition
        {
            get
            {
                return _audioFileReader != null ? _audioFileReader.Position : 0;
            }
            set
            {
                if (_audioFileReader == null)
                {
                    return;
                }
                _audioFileReader.Position = value;
            }
        }

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
