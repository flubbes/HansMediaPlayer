using Hans.Components.Database;
using Hans.Core.Database.Playlists;
using Hans.Core.Database.Songs;
using Hans.Database.Playlists.FlatFile;
using Ninject.Modules;

namespace Hans.Gui.WinForms.Modules
{
    public class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IPlaylistStore>().To<FlatFilePlaylistStore>();
            Kernel.Bind<ISongStore>().To<FlatFileSongStore>();
        }
    }
}
