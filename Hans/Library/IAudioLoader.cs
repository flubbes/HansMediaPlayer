﻿using NAudio.Wave;

namespace Hans.Library
{
    public interface IAudioLoader
    {
        /// <summary>
        /// Prepares the audio file to play
        /// </summary>
        WaveStream Load();

        /// <summary>
        /// The method that gets called at the end to wipe all data within the player
        /// </summary>
        void UnLoad();
    }
}