using System.Collections.Generic;
using Hans.Database;
using Hans.General;
using Hans.Properties;
using Ninject;

namespace Hans.Library
{
    public class HansMusicLibrary
    {
        private readonly List<HansPlaylist> playLists;
        private readonly List<HansSong> _songs;


        public HansMusicLibrary(IDatabaseSaver databaseSaver, ExitTrigger exitTrigger)
        {
            DatabaseSaver = databaseSaver;
            playLists = new List<HansPlaylist>();
            _songs = new List<HansSong>();
            exitTrigger.ExitTriggered += exitTrigger_ExitTriggered;
        }

        void exitTrigger_ExitTriggered()
        {
            SaveDatabase(DatabaseSaver);
        }

        public IDatabaseSaver DatabaseSaver { get; set; }

        public IEnumerable<HansPlaylist> Playlists
        {
            get { return playLists; }
        }

        public IEnumerable<HansSong> Songs
        {
            get { return _songs; }
        }

        public void CreatePlayList(string name)
        {
            playLists.Add(new HansPlaylist(name));
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