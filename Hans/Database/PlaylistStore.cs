using System.Collections.Generic;
using Hans.Library;

namespace Hans.Database
{
    public class PlaylistStore : IStore<HansPlaylist>
    {
        private readonly IPlaylistStore _playlistStore;

        public PlaylistStore(IPlaylistStore playlistStore)
        {
            _playlistStore = playlistStore;
        }

        public void Add(HansPlaylist item)
        {
            _playlistStore.Add(item);
        }

        public void Remove(HansPlaylist item)
        {
            _playlistStore.Remove(item);
        }

        public IEnumerable<HansPlaylist> GetEnumerable()
        {
            return _playlistStore.GetEnumerable();
        }

        public void Update(HansPlaylist item)
        {
            _playlistStore.Update(item);
        }
    }
}