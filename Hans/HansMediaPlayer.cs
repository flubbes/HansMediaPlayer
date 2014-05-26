using System.Collections.Generic;
using System.Linq;
using Hans.Library;
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
}
