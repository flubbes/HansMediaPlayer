using System.Collections.Generic;
using Hans.Components.Database;
using Hans.Core.Database.Playlists;
using Hans.Core.General;

namespace Hans.Database.Playlists.FlatFile
{
    public class FlatFilePlaylistStore : IPlaylistStore
    {
        private readonly FlatFileStorage<HansPlaylist> _flatFileStorage;
        /// <summary>
        /// Initalizes a new instance of the flatfile playlist store
        /// </summary>
        public FlatFilePlaylistStore(ExitAppTrigger exitAppTrigger)
        {
            _flatFileStorage = new FlatFileStorage<HansPlaylist>(
                StaticSettings.DatabasePlaylistStorePath, exitAppTrigger);
        }

        /// <summary>
        /// Add a new playlist to the store
        /// </summary>
        /// <param name="item"></param>
        public void Add(HansPlaylist item)
        {
            _flatFileStorage.Add(item);
        }

        /// <summary>
        /// removes an item the store
        /// </summary>
        /// <param name="item"></param>
        public void Remove(HansPlaylist item)
        {
            _flatFileStorage.Remove(item);
        }

        /// <summary>
        /// Updates an item from the store
        /// </summary>
        /// <param name="item"></param>
        public void Update(HansPlaylist item)
        {
            _flatFileStorage.Update(item);
        }

        /// <summary>
        /// Gets the enumerable from the store
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HansPlaylist> GetEnumerable()
        {
            return _flatFileStorage.GetEnumerable();
        }
    }
}