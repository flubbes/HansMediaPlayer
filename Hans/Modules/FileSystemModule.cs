using Hans.FileSystem;
using Hans.FileSystem.Default;
using Ninject.Modules;

namespace Hans.Modules
{
    internal class FileSystemModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IExists>().To<DefaultExists>();
            Bind<IGet>().To<DefaultGet>();
            Bind<IOpen>().To<DefaultOpen>();
            Bind<IReadWrite>().To<DefaultReadWrite>();
        }
    }
}