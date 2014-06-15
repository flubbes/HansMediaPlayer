using Hans.Database.Songs;
using Hans.General;
using Hans.Library;
using NAudio.Wave;
using System;
using System.Threading;
using System.Windows;

namespace Hans.Audio
{
    /// <summary>
    /// The audioplayer class for hans
    /// </summary>
    public class AudioPlayer : IAudioPlayer, IDisposable
    {
        private readonly IAudioLoader _audioLoader;
        private HansSong _current;
        private bool _exit;
        private bool _loadCurrent;
        private long _newPosition;
        private float _newVolume;
        private bool _pause;
        private bool _reload;
        private bool _setPosition;
        private bool _setVolume;
        private bool _stop;
        private IWavePlayer _wavePlayer;

        /// <summary>
        /// Initializes the audio player class
        /// </summary>
        /// <param name="audioLoader"></param>
        /// <param name="exitAppTrigger"></param>
        public AudioPlayer(IAudioLoader audioLoader, ExitAppTrigger exitAppTrigger)
        {
            _audioLoader = audioLoader;
            exitAppTrigger.GotTriggered += exitAppTrigger_GotTriggered;
            new Thread(PlayerThread).Start();
        }

        /// <summary>
        /// Gets triggered when the load fails
        /// </summary>
        public event EventHandler LoadingFailed;

        /// <summary>
        /// Gets triggered when the currently playing song finished playing
        /// </summary>
        public event EventHandler SongFinished;

        /// <summary>
        /// Gets triggered when a new songs started playing
        /// </summary>
        public event StartedPlayingEventHandler StartedPlaying;

        /// <summary>
        /// Gets the length of the currently playling song
        /// </summary>
        public double Length { get; private set; }

        /// <summary>
        /// The current playback state
        /// </summary>
        public PlaybackState PlaybackState
        {
            get { return _wavePlayer.PlaybackState; }
        }

        /// <summary>
        /// Gets the current song position
        /// </summary>
        public long Position { get; private set; }

        /// <summary>
        /// Gets or sets the volume
        /// </summary>
        public float Volume { get; private set; }

        /// <summary>
        /// Disposes the audioplayer and all of its native and managed inherited objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Pauses the audio player
        /// </summary>
        public void Pause()
        {
            _pause = true;
        }

        /// <summary>
        /// Plays a new song
        /// </summary>
        /// <param name="song"></param>
        public void Play(HansSong song)
        {
            if (PlaybackState == PlaybackState.Stopped)
            {
                _current = song;
                _loadCurrent = true;
            }
            else
            {
                _current = song;
                _reload = true;
            }
        }

        /// <summary>
        /// Sets the current song position
        /// </summary>
        /// <param name="value"></param>
        public void SetPosition(long value)
        {
            _newPosition = value;
            _setPosition = true;
        }

        /// <summary>
        /// Sets the volume
        /// </summary>
        /// <param name="value"></param>
        public void SetVolume(float value)
        {
            _newVolume = value;
            _setVolume = true;
        }

        /// <summary>
        /// Stops the the playback
        /// </summary>
        public void Stop()
        {
            if (PlaybackState == PlaybackState.Playing)
            {
                _stop = true;
            }
        }

        /// <summary>
        /// Triggers the loading fail event
        /// </summary>
        protected virtual void OnLoadingFailed()
        {
            var handler = LoadingFailed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Triggers the song finished event
        /// </summary>
        protected virtual void OnSongFinished()
        {
            var handler = SongFinished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Triggers the started playing event
        /// </summary>
        /// <param name="song"></param>
        protected virtual void OnStartedPlaying(HansSong song)
        {
            var handler = StartedPlaying;
            if (handler != null)
            {
                handler(this, new StartedPlayingEventArgs { Song = song });
            }
        }

        /// <summary>
        /// Disposes managed and native inherited objects
        /// </summary>
        /// <param name="cleanAll"></param>
        private void Dispose(bool cleanAll)
        {
            if (cleanAll)
            {
            }
            _wavePlayer.Dispose();
        }

        /// <summary>
        /// Gets called when the exitAppTrigger got triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void exitAppTrigger_GotTriggered(object sender, EventArgs eventArgs)
        {
            _exit = true;
            _stop = true;
        }

        /// <summary>
        /// Handles the pause logic while the player is already paused
        /// </summary>
        private void HandlePauseWhilePasued()
        {
            if (_pause && PlaybackState == PlaybackState.Paused)
            {
                _wavePlayer.Play();
                _pause = false;
            }
        }

        /// <summary>
        /// Handles the pause logic while the player is currently playing
        /// </summary>
        private void HandlePauseWhilePlaying()
        {
            if (_pause && PlaybackState == PlaybackState.Playing)
            {
                _wavePlayer.Pause();
                _pause = false;
            }
        }

        /// <summary>
        /// Handles the playback
        /// </summary>
        private void HandlePlayBack()
        {
            using (var audioFileReader = _audioLoader.Load(_current))
            {
                StartPlayer(audioFileReader);
                while (_wavePlayer.PlaybackState != PlaybackState.Stopped)
                {
                    HandlePlayEvents(audioFileReader);
                    Thread.Sleep(50);
                }
            }
            if (!_reload)
            {
                OnSongFinished();
            }
            _reload = false;
        }

        /// <summary>
        /// Handles the events while playing
        /// </summary>
        /// <param name="audioFileReader"></param>
        private void HandlePlayEvents(AudioFileReader audioFileReader)
        {
            HandleSetVolume(audioFileReader);
            HandleSetPosition(audioFileReader);
            HandlePauseWhilePlaying();
            HandlePauseWhilePasued();
            HandleStop();
            HandleReload();
            SetPlayerProperties(audioFileReader);
        }

        /// <summary>
        /// Handles the reload
        /// </summary>
        private void HandleReload()
        {
            if (_reload)
            {
                _wavePlayer.Stop();
                _loadCurrent = true;
            }
        }

        /// <summary>
        /// Handles the set position event
        /// </summary>
        /// <param name="audioFileReader"></param>
        private void HandleSetPosition(AudioFileReader audioFileReader)
        {
            if (_setPosition)
            {
                audioFileReader.Position = _newPosition;
                _setPosition = false;
            }
        }

        /// <summary>
        /// Handles the set volume event
        /// </summary>
        /// <param name="audioFileReader"></param>
        private void HandleSetVolume(AudioFileReader audioFileReader)
        {
            if (_setVolume)
            {
                audioFileReader.Volume = _newVolume;
                Volume = _newVolume;
                _setVolume = false;
            }
        }

        /// <summary>
        /// Handles the stop event
        /// </summary>
        private void HandleStop()
        {
            if (_stop)
            {
                _wavePlayer.Stop();
                _stop = false;
            }
        }

        /// <summary>
        /// The thread that handles all the playing logic
        /// </summary>
        private void PlayerThread()
        {
            _wavePlayer = new WaveOut();
            while (!_exit)
            {
                if (_loadCurrent)
                {
                    _loadCurrent = false;
                    try
                    {
                        HandlePlayBack();
                    }
                    catch
                    {
                        OnLoadingFailed();
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Sets all player properties
        /// </summary>
        /// <param name="audioFileReader"></param>
        private void SetPlayerProperties(AudioFileReader audioFileReader)
        {
            Length = audioFileReader.Length;
            Position = audioFileReader.Position;
            audioFileReader.Volume = Volume;
        }

        /// <summary>
        /// Starts the player to play a new song
        /// </summary>
        /// <param name="audioFileReader"></param>
        private void StartPlayer(AudioFileReader audioFileReader)
        {
            _wavePlayer.Init(audioFileReader);
            _wavePlayer.Play();
            SetPlayerProperties(audioFileReader);
            OnStartedPlaying(_current);
        }
    }
}