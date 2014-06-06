using System.Collections.Generic;
using Hans.Database.FlatFile;
using Hans.Properties;

namespace Hans.Database.PlaylistSongs.FlatFile
{
    public class FlatFilePlaylistSongStore : IPlaylistSongStore
    {
        private readonly FlatFileStorage<PlaylistSong> _flatFileStorage;

        public FlatFilePlaylistSongStore()
        {
            _flatFileStorage = new FlatFileStorage<PlaylistSong>(
                Settings.Default.Database_PlaylistSongStore_Path);
        }

        public void Add(PlaylistSong item)
        {
            _flatFileStorage.Add(item);
        }

        public void Remove(PlaylistSong item)
        {
            _flatFileStorage.Remove(item);
        }

        public IEnumerable<PlaylistSong> GetEnumerable()
        {
            return _flatFileStorage.GetEnumerable();
        }

        public void Update(PlaylistSong item)
        {
            _flatFileStorage.Update(item);
        }
    }
}
