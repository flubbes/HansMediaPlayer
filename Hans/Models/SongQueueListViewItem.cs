using System;

namespace Hans.Models
{
    /// <summary>
    /// The song queue list view item model for the form
    /// </summary>
    public class SongQueueListViewItem
    {
        private string _artist;

        /// <summary>
        /// The artist with playing indicator
        /// </summary>
        public string Artist
        {
            get { return CurrentlyPlaying ? "♥ " + _artist : _artist; }
            set { _artist = value; }
        }

        /// <summary>
        /// If the song is currently playing
        /// </summary>
        public bool CurrentlyPlaying { get; set; }

        /// <summary>
        /// THe length of the song
        /// </summary>
        public TimeSpan Length { get; set; }

        /// <summary>
        /// The title of the song
        /// </summary>
        public string Title { get; set; }
    }
}