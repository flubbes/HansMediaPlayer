using System.Collections.Generic;

namespace Hans.Library
{
    public class HansPlaylist
    {
        private volatile List<HansSong> _songs;

        public HansPlaylist(string name)
        {
            _songs = new List<HansSong>();
            Name = name;
        }

        public void Add(HansSong hansSong)
        {
            _songs.Add(hansSong);
        }

        public IEnumerable<HansSong> Songs
        {
            get { return _songs; }
        }

        public string Name { get; set; }

        public void Remove(HansSong hansSong)
        {
            _songs.Remove(hansSong);
        }
    }
}