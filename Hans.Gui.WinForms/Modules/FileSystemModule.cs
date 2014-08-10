using Hans.Components.FileSystem;
using Hans.Core.FileSystem;
using Ninject.Modules;

namespace Hans.Gui.WinForms.Modules
{
    public class FileSystemModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IReadWrite>().To<DefaultReadWrite>();
            Kernel.Bind<IOpen>().To<DefaultOpen>();
            Kernel.Bind<IGet>().To<DefaultGet>();
            Kernel.Bind<IExists>().To<DefaultExists>();
        }
    }
}
