using Hans.Database.Songs;
using Hans.General;
using NAudio.Wave;
using System;

namespace Hans.Audio
{
    public interface IAudioPlayer
    {
        /// <summary>
        /// Gets triggered when the loading failed
        /// </summary>
        event EventHandler LoadingFailed;

        /// <summary>
        /// Gets triggered when a song finished playing
        /// </summary>
        event EventHandler SongFinished;

        /// <summary>
        /// Gets triggered when a song started playing
        /// </summary>
        event StartedPlayingEventHandler StartedPlaying;

        /// <summary>
        /// The length of the currently playing song
        /// </summary>
        double Length { get; }

        /// <summary>
        /// Gets the current playback state
        /// </summary>
        PlaybackState PlaybackState { get; }

        /// <summary>
        /// Gets the position in the currently playing song
        /// </summary>
        long Position { get; }

        /// <summary>
        /// Gets the volume
        /// </summary>
        float Volume { get; }

        /// <summary>
        /// Pauses the player
        /// </summary>
        void Pause();

        /// <summary>
        /// Plays a new song
        /// </summary>
        /// <param name="song"></param>
        void Play(HansSong song);

        /// <summary>
        /// Sets the position of the currently playing song
        /// </summary>
        /// <param name="value"></param>
        void SetPosition(long value);

        /// <summary>
        /// Sets the volume of the player
        /// </summary>
        /// <param name="value"></param>
        void SetVolume(float value);

        /// <summary>
        /// Stops the player
        /// </summary>
        void Stop();
    }
}