using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Hans
{
    public class HansMediaPlayer
    {
        private IWavePlayer player;
        private volatile Queue<HansSong> _songQueue;

        public HansMediaPlayer()
        {
            _songQueue = new Queue<HansSong>();
            player = new WaveOut();
        }

        public void Pause()
        {
            
        }

        public void Play()
        {
            if (IsQueueEmpty())
                return;

        }

        public HansSong CurrentlyPlaying
        {
            get
            {
                if (IsQueueEmpty())
                    return null;
                return _songQueue.Peek();
            }
        }

        private bool IsQueueEmpty()
        {
            return !_songQueue.Any();
        }

        public void Stop()
        {
            
        }

        public Queue<HansSong> SongQueue
        {
            get
            {
                lock (_songQueue)
                {
                    return _songQueue;
                }
            }
        }
    }

    public class HansSong
    {

        public HansSong(string path)
        {
            Path = path;
        }

        public long Length
        {
            get { return 0; }
        }

        public long Position
        {
            get { return 0; }
        }

        public String Path { get; set; }
    }
}
