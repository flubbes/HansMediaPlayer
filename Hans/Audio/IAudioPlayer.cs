using Hans.Database.Songs;
using Hans.General;
using NAudio.Wave;
using System;

namespace Hans.Audio
{
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
}