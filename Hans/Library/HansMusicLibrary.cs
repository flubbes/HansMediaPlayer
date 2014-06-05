using System.Collections.Generic;
using Hans.Database;
using Hans.General;
using Hans.Properties;
using Ninject;

namespace Hans.Library
{
    public class HansMusicLibrary
    {
        private readonly List<HansPlaylist> _playLists;
        private readonly List<HansSong> _songs;

        /// <summary>
        /// Creates a new instance of the hans music library
        /// </summary>
        /// <param name="databaseSaver"></param>
        /// <param name="exitAppTrigger"></param>
        /// <param name="?"></param>
        public HansMusicLibrary(IDatabaseSaver databaseSaver, ExitAppTrigger exitAppTrigger)
        {
            DatabaseSaver = databaseSaver;
            _playLists = new List<HansPlaylist>();
            _songs = new List<HansSong>();
            exitAppTrigger.GotTriggered += GotTriggerGotTriggered;
        }

        void GotTriggerGotTriggered()
        {
            SaveDatabase(DatabaseSaver);
        }

        public IDatabaseSaver DatabaseSaver { get; set; }

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

        public void SaveDatabase(IDatabaseSaver databaseSaver)
        {
            databaseSaver.Save(this, Settings.Default.Database_Path);
        }
    }
}