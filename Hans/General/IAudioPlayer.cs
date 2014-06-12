using Hans.Database.Songs;
using Hans.Library;
using NAudio.Wave;
using System;
using System.Threading;

namespace Hans.General
{
    public delegate void StartedPlayingEventHandler(HansSong song);

    public interface IAudioPlayer
    {
        event EventHandler LoadingFailed;

        event EventHandler SongFinished;

        event StartedPlayingEventHandler StartedPlaying;

        double Length { get; }

        PlaybackState PlaybackState { get; }

        long Position { get; set; }

        float Volume { get; }

        void Pause();

        void Play(HansSong song);

        void SetPosition(long value);

        void SetVolume(float value);

        void Stop();
    }

    public class AudioPlayer : IAudioPlayer
    {
        private readonly IAudioLoader _audioLoader;
        private readonly IWavePlayer _wavePlayer;
        private HansSong _current;
        private bool _loadCurrent;
        private long _newPosition;
        private float _newVolume;
        private bool _pause;
        private bool _reload;
        private bool _setPosition;
        private bool _setVolume;
        private bool _stop;

        public AudioPlayer(IAudioLoader audioLoader)
        {
            _audioLoader = audioLoader;
            _wavePlayer = new WaveOut();
            new Thread(PlayerThread) { IsBackground = true }.Start();
        }

        public event EventHandler LoadingFailed;

        public event EventHandler SongFinished;

        public event StartedPlayingEventHandler StartedPlaying;

        public double Length { get; private set; }

        public PlaybackState PlaybackState
        {
            get { return _wavePlayer.PlaybackState; }
        }

        public long Position { get; set; }

        public float Volume { get; private set; }

        public void Pause()
        {
            _pause = true;
        }

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

        public void SetPosition(long value)
        {
            _newPosition = value;
            _setPosition = true;
        }

        public void SetVolume(float value)
        {
            _newVolume = value;
            _setVolume = true;
        }

        public void Stop()
        {
            if (PlaybackState == PlaybackState.Playing)
            {
                _stop = true;
            }
        }

        protected virtual void OnLoadingFailed()
        {
            var handler = LoadingFailed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSongFinished()
        {
            var handler = SongFinished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStartedPlaying(HansSong song)
        {
            var handler = StartedPlaying;
            if (handler != null)
            {
                handler(song);
            }
        }

        private void HandlePauseWhilePasued()
        {
            if (_pause && PlaybackState == PlaybackState.Paused)
            {
                _wavePlayer.Play();
                _pause = false;
            }
        }

        private void HandlePauseWhilePlaying()
        {
            if (_pause && PlaybackState == PlaybackState.Playing)
            {
                _wavePlayer.Pause();
                _pause = false;
            }
        }

        private void HandlePlayBack()
        {
            using (var audioFileReader = _audioLoader.Load(_current))
            {
                StartPlayer(audioFileReader);
                while (_wavePlayer.PlaybackState != PlaybackState.Stopped)
                {
                    HandlePlayEvents(audioFileReader);
                    Thread.Sleep(1);
                }
            }
            if (!_reload)
            {
                OnSongFinished();
            }
            _reload = false;
        }

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

        private void HandleReload()
        {
            if (_reload)
            {
                _wavePlayer.Stop();
                _loadCurrent = true;
            }
        }

        private void HandleSetPosition(AudioFileReader audioFileReader)
        {
            if (_setPosition)
            {
                audioFileReader.Position = _newPosition;
                _setPosition = false;
            }
        }

        private void HandleSetVolume(AudioFileReader audioFileReader)
        {
            if (_setVolume)
            {
                audioFileReader.Volume = _newVolume;
                _setVolume = false;
            }
        }

        private void HandleStop()
        {
            if (_stop)
            {
                _wavePlayer.Stop();
                _stop = false;
            }
        }

        private void PlayerThread()
        {
            while (true)
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

        private void SetPlayerProperties(AudioFileReader audioFileReader)
        {
            Length = audioFileReader.Length;
            Position = audioFileReader.Position;
            Volume = audioFileReader.Volume;
        }

        private void StartPlayer(AudioFileReader audioFileReader)
        {
            _wavePlayer.Init(audioFileReader);
            _wavePlayer.Play();
            SetPlayerProperties(audioFileReader);
            OnStartedPlaying(_current);
        }
    }
}