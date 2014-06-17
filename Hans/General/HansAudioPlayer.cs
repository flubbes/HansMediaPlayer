using System.Diagnostics;
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

        private Boolean muted = false;
        private float previousVolume;

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
                PreviousVolume = Volume;
                _audioPlayer.SetVolume(value);
            }
        }

        public Boolean Muted { get; set; }
        public float PreviousVolume { get; set; }

        /// <summary>
        /// Adds a song to the current playlist
        /// </summary>
        /// <param name="hansSong"></param>
        public void AddToCurrentPlayList(HansSong hansSong)
        {
            _songQueue.Add(hansSong);
        }

        /// <summary>
        /// Downloads a new song
        /// </summary>
        /// <param name="track"></param>
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

        /// <summary>
        /// Determines whether hans is currently playing a song
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public bool IsCurrentPlayingSong(HansSong song)
        {
            return _audioPlayer.PlaybackState == PlaybackState.Playing && _songQueue.ElementAt(_listPosition).Equals(song);
        }

        /// <summary>
        /// Loads a folder to the library
        /// </summary>
        /// <param name="path"></param>
        public void LoadFolder(string path)
        {
            Library.AddFolder(new FolderAddRequest
            {
                Path = path,
            });
        }

        /// <summary>
        /// Play the next song
        /// </summary>
        public void Next()
        {
            PlayNextSong();
        }

        /// <summary>
        /// Pauses the playback
        /// </summary>
        public void Pause()
        {
            _audioPlayer.Pause();
        }

        /// <summary>
        /// Plays the next song in the queue
        /// </summary>
        public void Play()
        {
            if (IsQueueEmpty())
            {
                return;
            }
            var song = _songQueue[_listPosition];
            _audioPlayer.Play(song);
        }

        /// <summary>
        /// Goes one song back
        /// </summary>
        public void Previous()
        {
            PlayPreviousSong();
        }

        /// <summary>
        /// Searches online
        /// </summary>
        /// <param name="searchRequest"></param>
        public void Search(SearchRequest searchRequest)
        {
            new Thread(() => StartSearch(searchRequest))
            {
                IsBackground = true
            }.Start();
        }

        /// <summary>
        /// Sets the playing index in the current song queue
        /// </summary>
        /// <param name="value"></param>
        public void SetPlayingIndex(int value)
        {
            _listPosition = value;
            Play();
        }

        /// <summary>
        /// stop the playback
        /// </summary>
        public void Stop()
        {
            _audioPlayer.Stop();
        }

        /// <summary>
        /// Triggers the new song event
        /// </summary>
        protected virtual void OnNewSong()
        {
            var handler = NewSong;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            setWindowsTitle();
        }

        private void setWindowsTitle()
        {
            // Retrieve current song name
            var title = _songQueue[_listPosition].Title;
            // Set current song title
            Debug.WriteLine(title);
        }

        /// <summary>
        /// Gets called when the songdownloads class downloaded a new song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void _songDownloads_DownloadFinished(object sender, DownloadFinishedEventArgs args)
        {
            _songQueue.Add(BuildHansSongFromDownloadRequest(args));
            OnSongQueueChanged();
        }

        /// <summary>
        /// Gets called when the audioplayer couldn't play a song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioPlayer_LoadingFailed(object sender, EventArgs e)
        {
            Next();
        }

        /// <summary>
        /// Gets called when the audio player finished a song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioPlayer_SongFinished(object sender, EventArgs e)
        {
            Next();
            OnSongQueueChanged();
        }

        /// <summary>
        /// Gets called when the audio player started playing a new song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void audioPlayer_StartedPlaying(object sender, StartedPlayingEventArgs e)
        {
            OnNewSong();
            OnSongQueueChanged();
        }

        /// <summary>
        /// Converts a downloadsRequest to a hanssong
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private HansSong BuildHansSongFromDownloadRequest(DownloadFinishedEventArgs args)
        {
            var directory = args.DownloadRequest.DestinationDirectory;
            var fileName = args.DownloadRequest.FileName;
            var fullPath = _fileSystem.Get.CombinationFullPath(directory, fileName);
            return HansSong.FromOnlineServiceTrack(fullPath, args.DownloadRequest.OnlineServiceTrack);
        }

        /// <summary>
        /// Builds the next position in the song queue
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Builds the previous position in the song queue
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// If the current index is the index at the end
        /// </summary>
        /// <returns></returns>
        private bool IsAtEndOfSongQueue()
        {
            return _listPosition == _songQueue.Count - 1;
        }

        /// <summary>
        /// if the current index is the index at the beginning
        /// </summary>
        /// <returns></returns>
        private bool IsAtStartOfSongQueue()
        {
            return _listPosition == 0;
        }

        /// <summary>
        /// If the song queue is empty
        /// </summary>
        /// <returns></returns>
        private bool IsQueueEmpty()
        {
            return !_songQueue.Any();
        }

        /// <summary>
        /// When the search is finished
        /// </summary>
        /// <param name="tracks"></param>
        private void OnSearchFinished(IEnumerable<IOnlineServiceTrack> tracks)
        {
            if (SearchFinished != null)
            {
                SearchFinished(this, new SearchFinishedEventArgs { Tracks = tracks });
            }
        }

        /// <summary>
        /// Triggers the songqueue changed event
        /// </summary>
        private void OnSongQueueChanged()
        {
            if (SongQueueChanged != null)
            {
                SongQueueChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Plays the next song
        /// </summary>
        private void PlayNextSong()
        {
            if (!IsQueueEmpty() && BuildNextPosition())
            {
                Play();
            }
        }

        /// <summary>
        /// Plays the previous song
        /// </summary>
        private void PlayPreviousSong()
        {
            if (!IsQueueEmpty() && BuildPreviousPosition())
            {
                Play();
            }
        }

        /// <summary>
        /// Starts an online search
        /// </summary>
        /// <param name="searchRequest"></param>
        private void StartSearch(SearchRequest searchRequest)
        {
            OnSearchFinished(searchRequest.OnlineService.Search(searchRequest.Query));
        }
    }
}