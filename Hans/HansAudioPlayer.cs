using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hans.Library;
using Hans.Tests;
using NAudio.Wave;

namespace Hans
{
    public class HansAudioPlayer
    {
        private volatile List<HansSong> _songQueue;
        private volatile int _listPosition;

        public delegate void SearchFinishedEventHandler(IEnumerable<IOnlineServiceTrack> tracks);

        public event SearchFinishedEventHandler SearchFinished;

        public HansAudioPlayer()
        {
            _listPosition = 0;
            _songQueue = new List<HansSong>();
            Player = new WaveOut();
            
        }

        public void Pause()
        {
            
        }

        public void Play()
        {
            if (IsQueueEmpty())
                return;
        }

        public IWavePlayer Player { get; set; }

        public HansSong CurrentlyPlaying
        {
            get
            {
                return IsQueueEmpty() ? null : _songQueue.ElementAt(_listPosition);
            }
        }

        public bool Shuffle { get; set; }

        public bool Reapeat { get; set; }

        public bool IsPlaying { get; private set; }

        private bool IsQueueEmpty()
        {
            return !_songQueue.Any();
        }

        public void Stop()
        {
            
        }

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

        public float Volume { get; set; }

        public void Previous()
        {
            
        }

        public void Next()
        {
            
        }

        public void Search(SearchRequest searchRequest)
        {
            new Thread(() => StartSearch(searchRequest))
            {
                IsBackground = true
            }.Start();
        }

        private void OnSearchFinished(IEnumerable<IOnlineServiceTrack> tracks)
        {
            if (SearchFinished != null)
            {
                SearchFinished(tracks);
            }
        }

        private void StartSearch(SearchRequest searchRequest)
        {
            OnSearchFinished(searchRequest.OnlineService.Search(searchRequest.Query));
        }
    }
}
