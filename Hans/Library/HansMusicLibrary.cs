using System.Collections.Generic;
using Hans.Database;
using Hans.General;
using Hans.Properties;
using Ninject;

namespace Hans.Library
{
    public class HansMusicLibrary
    {
        private readonly IDatabaseLoader _databaseLoader;
        private List<HansPlaylist> _playLists;
        private List<HansSong> _songs;
        private readonly IDatabaseSaver _databaseSaver;

        /// <summary>
        /// Creates a new instance of the hans music library
        /// </summary>
        /// <param name="databaseSaver"></param>
        /// <param name="exitAppTrigger"></param>
        /// <param name="?"></param>
        public HansMusicLibrary(IDatabaseSaver databaseSaver, ExitAppTrigger exitAppTrigger, IDatabaseLoader databaseLoader)
        {
            _databaseLoader = databaseLoader;
            _databaseSaver = databaseSaver;
            InitializeProperties();
            exitAppTrigger.GotTriggered += GotTriggerGotTriggered;
        }

        private void InitializeProperties()
        {
            _playLists = new List<HansPlaylist>();
            _songs = new List<HansSong>();
        }

        void GotTriggerGotTriggered()
        {
            SaveDatabase(_databaseSaver);
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

        public void SaveDatabase(IDatabaseSaver databaseSaver)
        {
            databaseSaver.Save(this, Settings.Default.Database_Path);
        }
    }
}