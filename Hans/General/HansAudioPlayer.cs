using Hans.Audio;
using Hans.Database.Songs;
using Hans.Library;
using Hans.Properties;
using Hans.Services;
using Hans.Web;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hans.General
{
    /// <summary>
    /// The base class from the hans audio player
    /// </summary>
    public class HansAudioPlayer
    {
        private readonly FileSystem.FileSystem _fileSystem;
        private readonly SongDownloads _songDownloads;
        private IAudioPlayer _audioPlayer;
        private volatile int _listPosition;
        private volatile List<HansSong> _songQueue;

        /// <summary>
        /// Initializes a new instance of the hans audio player
        /// </summary>
        /// <param name="library"></param>
        /// <param name="audioPlayer"></param>
        /// <param name="songDownloads"></param>
        /// <param name="fileSystem"></param>
        public HansAudioPlayer(HansMusicLibrary library, IAudioPlayer audioPlayer, SongDownloads songDownloads, FileSystem.FileSystem fileSystem)
        {
            _songQueue = new List<HansSong>();
            _audioPlayer = audioPlayer;
            _songDownloads = songDownloads;
            _fileSystem = fileSystem;
            audioPlayer.StartedPlaying += audioPlayer_StartedPlaying;
            audioPlayer.LoadingFailed += audioPlayer_LoadingFailed;
            audioPlayer.SongFinished += audioPlayer_SongFinished;
            Library = library;
            _listPosition = 0;
            _songDownloads.DownloadFinished += _songDownloads_DownloadFinished;
        }

        /// <summary>
        /// When a new song is playing
        /// </summary>
        public event EventHandler NewSong;

        /// <summary>
        /// When the search finished
        /// </summary>
        public event SearchFinishedEventHandler SearchFinished;

        /// <summary>
        /// When the song queue changed
        /// </summary>
        public event SongQueueChangedEventHandler SongQueueChanged;

        /// <summary>
        /// The current song length of the song that is currently playing
        /// </summary>
        public double CurrentSongLength
        {
            get { return _audioPlayer.Length; }
        }

        /// <summary>
        /// The current song position
        /// </summary>
        public long CurrentSongPosition
        {
            get { return _audioPlayer.Position; }
            set
            {
                _audioPlayer.SetPosition(value);
            }
        }

        /// <summary>
        /// The hans music library
        /// </summary>
        public HansMusicLibrary Library { get; set; }

        /// <summary>
        /// Repeat the playlist at the end
        /// </summary>
        public bool Repeat { get; set; }

        /// <summary>
        /// Shuffle the songs whileplaying
        /// </summary>
        public bool Shuffle { get; set; }

        /// <summary>
        /// The downloads
        /// </summary>
        public SongDownloads SongDownloads
        {
            get { return _songDownloads; }
        }

        /// <summary>
        /// The songqueue --> current playlist
        /// </summary>
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

        /// <summary>
        /// The colume of the player
        /// </summary>
        public float Volume
        {
            get
            {
                return _audioPlayer.Volume;
            }
            set
            {
                _audioPlayer.SetVolume(value);
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
                ServiceName = track.ServiceName,
                Uri = track.Mp3Url,
                FileSystem = _fileSystem
            })) { IsBackground = true }.Start();
        }

        public bool IsCurrentPlayingSong(HansSong song)
        {
            return _audioPlayer.PlaybackState == PlaybackState.Playing && _songQueue.ElementAt(_listPosition).Equals(song);
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
            _audioPlayer.Pause();
        }

        public void Play()
        {
            if (IsQueueEmpty())
            {
                return;
            }
            var song = _songQueue[_listPosition];
            _audioPlayer.Play(song);
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

        public void SetPlayingIndex(int value)
        {
            _listPosition = value;
            Play();
        }

        public void Stop()
        {
            _audioPlayer.Stop();
        }

        protected virtual void OnNewSong()
        {
            var handler = NewSong;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void _songDownloads_DownloadFinished(object sender, DownloadFinishedEventHandlerArgs args)
        {
            _songQueue.Add(BuildHansSongFromDownloadRequest(args));
            OnSongQueueChanged();
        }

        private void audioPlayer_LoadingFailed(object sender, EventArgs e)
        {
            Next();
        }

        private void audioPlayer_SongFinished(object sender, EventArgs e)
        {
            Next();
            OnSongQueueChanged();
        }

        private void audioPlayer_StartedPlaying(object sender, StartedPlayingEventArgs e)
        {
            OnNewSong();
            OnSongQueueChanged();
        }

        private HansSong BuildHansSongFromDownloadRequest(DownloadFinishedEventHandlerArgs args)
        {
            var directory = args.DownloadRequest.DestinationDirectory;
            var fileName = args.DownloadRequest.FileName;
            var fullPath = _fileSystem.Get.CombinationFullPath(directory, fileName);
            return HansSong.FromOnlineServiceTrack(fullPath, args.DownloadRequest.OnlineServiceTrack);
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
                SearchFinished(this, new SearchFinishedEventArgs { Tracks = tracks });
            }
        }

        private void OnSongQueueChanged()
        {
            if (SongQueueChanged != null)
            {
                SongQueueChanged(this, EventArgs.Empty);
            }
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