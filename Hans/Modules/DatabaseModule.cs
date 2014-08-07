using Hans.Components.Database;
using Hans.Core.Database.Playlists;
using Hans.Core.Database.PlaylistSongs;
using Hans.Core.Database.Songs;
using Hans.Database.Playlists;
using Hans.Database.Playlists.FlatFile;
using Ninject.Modules;

namespace Hans.Modules
{
    /// <summary>
    /// The database module that loads all components of the hans player
    /// </summary>
    public class DatabaseModule : NinjectModule
    {
        /// <summary>
        /// Loads all modules for the database
        /// </summary>
        public override void Load()
        {
            Bind<IPlaylistStore>().To<FlatFilePlaylistStore>().InSingletonScope();
            Bind<ISongStore>().To<FlatFileSongStore>().InSingletonScope();
            Bind<IPlaylistSongStore>().To<FlatFilePlaylistSongStore>().InSingletonScope();
        }
    }
}
