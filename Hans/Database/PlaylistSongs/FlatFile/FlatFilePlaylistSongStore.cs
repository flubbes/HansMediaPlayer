using Hans.Database.FlatFile;
using Hans.General;
using Hans.Properties;
using System.Collections.Generic;

namespace Hans.Database.PlaylistSongs.FlatFile
{
    public class FlatFilePlaylistSongStore : IPlaylistSongStore
    {
        private readonly FlatFileStorage<PlaylistSong> _flatFileStorage;

        /// <summary>
        /// Initializes a new flatfile playlist store
        /// </summary>
        /// <param name="exitAppTrigger"></param>
        public FlatFilePlaylistSongStore(ExitAppTrigger exitAppTrigger)
        {
            _flatFileStorage = new FlatFileStorage<PlaylistSong>(
                Settings.Default.Database_PlaylistSongStore_Path, exitAppTrigger);
        }

        /// <summary>
        /// Adds an item to the the storage
        /// </summary>
        /// <param name="item"></param>
        public void Add(PlaylistSong item)
        {
            _flatFileStorage.Add(item);
        }

        /// <summary>
        /// Gets the current collection from the storage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PlaylistSong> GetEnumerable()
        {
            return _flatFileStorage.GetEnumerable();
        }

        /// <summary>
        /// Removes an item from the storage
        /// </summary>
        /// <param name="item"></param>
        public void Remove(PlaylistSong item)
        {
            _flatFileStorage.Remove(item);
        }

        /// <summary>
        /// Updates an item from the store
        /// </summary>
        /// <param name="item"></param>
        public void Update(PlaylistSong item)
        {
            _flatFileStorage.Update(item);
        }
    }
}