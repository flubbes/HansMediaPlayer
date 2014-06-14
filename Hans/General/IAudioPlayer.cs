using Hans.Database.Songs;
using NAudio.Wave;
using System;

namespace Hans.General
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