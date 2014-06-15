using System;

namespace Hans
{
    public class SongQueueListViewItem
    {
        private string _artist;

        public string Artist
        {
            get { return CurrentlyPlaying ? "♥ " + _artist : _artist; }
            set { _artist = value; }
        }

        public bool CurrentlyPlaying { get; set; }

        public TimeSpan Length { get; set; }

        public string Title { get; set; }
    }
}