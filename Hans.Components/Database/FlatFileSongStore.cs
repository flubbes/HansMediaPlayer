using System.Collections.Generic;
using Hans.Core.Database.Songs;
using Hans.Core.General;

namespace Hans.Components.Database
{
    public class FlatFileSongStore : ISongStore
    {
        private readonly FlatFileStorage<HansSong> _flatFileStorage;

        /// <summary>
        /// Initializes a new flat file song store
        /// </summary>
        /// <param name="exitAppTrigger"></param>
        public FlatFileSongStore(ExitAppTrigger exitAppTrigger)
        {
            _flatFileStorage = new FlatFileStorage<HansSong>(
                StaticSettings.DatabaseSongStorePath, exitAppTrigger);
        }

        /// <summary>
        /// Adds a new hanssong to the store
        /// </summary>
        /// <param name="item"></param>
        public void Add(HansSong item)
        {
            _flatFileStorage.Add(item);
        }

        /// <summary>
        /// Gets the current hanssong collection from the store
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HansSong> GetEnumerable()
        {
            return _flatFileStorage.GetEnumerable();
        }

        /// <summary>
        /// Removes a hanssong from the store
        /// </summary>
        /// <param name="item"></param>
        public void Remove(HansSong item)
        {
            _flatFileStorage.Remove(item);
        }

        /// <summary>
        /// Updates a new hans song from the store
        /// </summary>
        /// <param name="item"></param>
        public void Update(HansSong item)
        {
            _flatFileStorage.Update(item);
        }
    }
}