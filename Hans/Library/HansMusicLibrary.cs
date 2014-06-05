using System.Collections.Generic;
using System.Linq;
using Hans.Database;
using Hans.General;
using Hans.Properties;
using Ninject;

namespace Hans.Library
{
    public class HansMusicLibrary
    {
        private readonly PlaylistStore _playlistStore;
        private List<HansPlaylist> _playLists;
        private List<HansSong> _songs;

        /// <summary>
        /// Creates a new instance of the hans music library
        /// </summary>
        /// <param name="databaseSaver"></param>
        /// <param name="exitAppTrigger"></param>
        /// <param name="?"></param>
        /// <param name="playlistStore"></param>
        public HansMusicLibrary(PlaylistStore playlistStore)
        {
            _playlistStore = playlistStore;
            InitializeProperties();
        }

        private void InitializeProperties()
        {
            _playLists = new List<HansPlaylist>();
            _songs = new List<HansSong>();
        }

        public IEnumerable<HansPlaylist> Playlists
        {
            get { return _playLists; }
        }

        public IEnumerable<HansSong> Songs
        {
            get { return _songs; }
        }

        public void CreatePlayList(string name)
        {
            _playLists.Add(new HansPlaylist(name));
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