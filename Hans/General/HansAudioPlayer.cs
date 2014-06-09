using Hans.Database.Songs;
using Hans.Library;
using Hans.Properties;
using Hans.Services;
using Hans.Web;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Hans.General
{
    public class HansAudioPlayer
    {
        private readonly SongDownloads _songDownloads;
        private IAudioPlayer _audioPlayer;
        private volatile int _listPosition;
        private volatile List<HansSong> _songQueue;

        public HansAudioPlayer(HansMusicLibrary library, IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
            Library = library;
            _listPosition = 0;
            _songQueue = new List<HansSong>();
            _songDownloads = new SongDownloads();
            _songDownloads.DownloadFinished += _songDownloads_DownloadFinished;
        }

        public delegate void NewSongEventHandler();

        public event NewSongEventHandler NewSong;

        public event SearchFinishedEventHandler SearchFinished;

        public event SongQueueChangedEventHandler SongQueueChanged;

        public HansSong CurrentlyPlaying
        {
            get
            {
                return IsQueueEmpty() ? null : _songQueue.ElementAt(_listPosition);
            }
        }

        public long CurrentProgress
        {
            get
            {
                return _audioFileReader != null ? _audioFileReader.Position * 100 / _audioFileReader.Length : 0;
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
                try
                {
                    return _audioFileReader != null ? _audioFileReader.Position : 0;
                }
                catch
                {
                    return 0;
                }
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

        public bool IsPlaying
        {
            get { return Player.PlaybackState == PlaybackState.Playing; }
        }

        public HansMusicLibrary Library { get; set; }

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

        public void AddToCurrentPlayList(HansSong hansSong)
        {
            _songQueue.Add(hansSong);
        }

        public void Download(IOnlineServiceTrack track)
        {
            new Thread(() => _songDownloads.Start(new DownloadRequest
            {
                DestinationDirectory = Settings.Default.Download_Temp_Directory,
                FileName = track.GetFileName(),
                OnlineServiceTrack = track,
                Downloader = track.GetDownloader(),
                Uri = track.Mp3Url
            })).Start();
        }

        public void LoadFolder(string path)
        {
            Library.AddFolder(new FolderAddRequest
            {
                Path = path,
            });
        }

        public void Next()
        {
            PlayNextSong();
        }

        public void Pause()
        {
            if (IsPlaying)
            {
                Player.Pause();
            }
        }

        public void Play()
        {
            if (IsQueueEmpty())
            {
                return;
            }

            var song = _songQueue[_listPosition];
            try
            {
                if (_audioFileReader != null)
                {
                    try
                    {
                        _audioFileReader.Close();
                        _audioFileReader.Dispose();
                    }
                    catch
                    {
                    }
                    _audioFileReader = null;
                }
                _audioFileReader = _audioLoader.Load(song);
                Player.Init(_audioFileReader);
                Player.Play();
                OnNewSong();
            }
            catch (Exception)
            {
                PlayNextSong();
            }
        }

        public void Previous()
        {
            PlayPreviousSong();
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
            Player.Stop();
        }

        protected virtual void OnNewSong()
        {
            var handler = NewSong;
            if (handler != null)
            {
                handler();
            }
        }

        private void _songDownloads_DownloadFinished(object sender, DownloadFinishedEventHandlerArgs args)
        {
            _songQueue.Add(
                    HansSong.FromOnlineServiceTrack(
                            args.DownloadRequest.GetAbsolutePath(),
                            args.DownloadRequest.OnlineServiceTrack
                        )
                );
            OnSongQueueChanged();
        }

        private bool BuildNextPosition()
        {
            if (Shuffle)
            {
                _listPosition = new Random().Next(0, _songQueue.Count);
            }
            else
            {
                if (IsAtEndOfSongQueue())
                {
                    if (!Repeat)
                    {
                        return false;
                    }
                    _listPosition = 0;
                }
                else
                {
                    _listPosition++;
                }
            }
            return true;
        }

        private bool BuildPreviousPosition()
        {
            if (Shuffle)
            {
                _listPosition = new Random().Next(0, _songQueue.Count);
            }
            else
            {
                if (IsAtStartOfSongQueue())
                {
                    if (!Repeat)
                    {
                        return false;
                    }
                    _listPosition = _songQueue.Count - 1;
                }
                else
                {
                    _listPosition--;
                }
            }
            return true;
        }

        private bool IsAtEndOfSongQueue()
        {
            return _listPosition == _songQueue.Count - 1;
        }

        private bool IsAtStartOfSongQueue()
        {
            return _listPosition == 0;
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

        private void Player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlayNextSong();
        }

        private void PlayNextSong()
        {
            if (!IsQueueEmpty() && BuildNextPosition())
            {
                Play();
            }
        }

        private void PlayPreviousSong()
        {
            if (!IsQueueEmpty() && BuildPreviousPosition())
            {
                Play();
            }
        }

        private void StartSearch(SearchRequest searchRequest)
        {
            OnSearchFinished(searchRequest.OnlineService.Search(searchRequest.Query));
        }
    }
}