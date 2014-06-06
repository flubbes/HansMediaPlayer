using System;
using System.Collections.Generic;
using Hans.Database.Playlists;
using Hans.Database.Songs;

namespace Hans.Library
{
    public class HansMusicLibrary
    {
        private readonly IPlaylistStore _playlistStore;
        private List<HansSong> _songs;

        /// <summary>
        /// Creates a new instance of the hans music library
        /// </summary>
        public HansMusicLibrary(IPlaylistStore playlistStore)
        {
            _playlistStore = playlistStore;
            InitializeProperties();
        }

        /// <summary>
        /// Initializes the properties of this class
        /// </summary>
        private void InitializeProperties()
        {
            _songs = new List<HansSong>();
        }

        /// <summary>
        /// Alls playlists in this library
        /// </summary>
        public IEnumerable<HansPlaylist> Playlists
        {
            get
            {
                return _playlistStore.GetEnumerable(); 
            }
        }

        public IEnumerable<HansSong> Songs
        {
            get
            {
                //TODO update to new format
                return _songs;
            }
        }

        /// <summary>
        /// Creates a new playlist
        /// </summary>
        /// <param name="name"></param>
        public void CreatePlayList(string name)
        {
            _playlistStore.Add(new HansPlaylist
            {
                Id = Guid.NewGuid(),
                Name = name
            });
        }

        public void AddSong(HansSong hansSong)
        {
            _songs.Add(hansSong);
        }

        public void RemoveSong(HansSong hansSong)
        {
            _songs.Remove(hansSong);
        }
    }
}