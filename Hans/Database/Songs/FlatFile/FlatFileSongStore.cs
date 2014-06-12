using System.Collections.Generic;
using Hans.Database.FlatFile;
using Hans.General;
using Hans.Properties;

namespace Hans.Database.Songs.FlatFile
{
    public class FlatFileSongStore : ISongStore
    {
        private readonly FlatFileStorage<HansSong> _flatFileStorage;

        public FlatFileSongStore(ExitAppTrigger exitAppTrigger)
        {
            _flatFileStorage = new FlatFileStorage<HansSong>(
                Settings.Default.Database_SongStore_Path, exitAppTrigger);
        }

        public void Add(HansSong item)
        {
            _flatFileStorage.Add(item);
        }

        public void Remove(HansSong item)
        {
            _flatFileStorage.Remove(item);
        }

        public IEnumerable<HansSong> GetEnumerable()
        {
            return _flatFileStorage.GetEnumerable();
        }

        public void Update(HansSong item)
        {
            _flatFileStorage.Update(item);
        }
    }
}
