using System.Collections.Generic;
using Hans.Database;
using Ninject;

namespace Hans.Library
{
    public class HansMusicLibrary
    {
        private List<HansPlaylist> playLists;
        private List<HansSong> _songs;


        public HansMusicLibrary()
        {
            playLists = new List<HansPlaylist>();
            _songs = new List<HansSong>();
        }

        [Inject]
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
            databaseSaver.Save(this);
        }
    }
}