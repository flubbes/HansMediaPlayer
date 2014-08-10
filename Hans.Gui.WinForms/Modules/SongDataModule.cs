using Hans.Core.SongData;
using Ninject.Modules;

namespace Hans.Gui.WinForms.Modules
{
    public class SongDataModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISongDataFinder>().To<SongDataFinder>();
        }
    }
}
