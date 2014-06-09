using Hans.Database.Songs;
using Hans.Library;
using NAudio.Wave;
using System;
using System.Threading;

namespace Hans.General
{
    public interface IAudioPlayer
    {
        void Pause();

        void Play(HansSong song);

        void Stop();
    }

    public class AudioPlayer : IAudioPlayer
    {
        private IAudioLoader _audioLoader;
        private bool _stop, _pause, _reload, _loadCurrent;
        private IWavePlayer _wavePlayer;
        private HansSong current;

        public AudioPlayer(IAudioLoader audioLoader)
        {
            _audioLoader = audioLoader;
            _wavePlayer = new WaveOut();
            new Thread(PlayerThread).Start();
        }

        public int Length { get; set; }

        public void Pause()
        {
            _pause = true;
        }

        public void Play(HansSong song)
        {
            current = song;
            _reload = true;
        }

        public void Stop()
        {
            _stop = true;
        }

        private void PlayerThread()
        {
            while (true)
            {
                if (_loadCurrent)
                {
                    using (var audioFileReader = _audioLoader.Load(current))
                    {
                        _wavePlayer.Init(audioFileReader);
                        _wavePlayer.Play();
                        while (_wavePlayer.PlaybackState != PlaybackState.Stopped)
                        {
                            if (_pause)
                            {
                                _wavePlayer.Pause();
                                _pause = false;
                            }
                            if (_stop)
                            {
                                _wavePlayer.Stop();
                            }
                            if (_reload)
                            {
                                _wavePlayer.Stop();
                                _loadCurrent = true;
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }
    }
}