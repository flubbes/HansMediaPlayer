using Hans.Components.FileSystem;
using Hans.Core.FileSystem;
using Ninject.Modules;

namespace Hans.Modules
{
    /// <summary>
    /// The file system module for hans
    /// </summary>
    internal class FileSystemModule : NinjectModule
    {
        /// <summary>
        /// Loads the module
        /// </summary>
        public override void Load()
        {
            Bind<IExists>().To<DefaultExists>();
            Bind<IGet>().To<DefaultGet>();
            Bind<IOpen>().To<DefaultOpen>();
            Bind<IReadWrite>().To<DefaultReadWrite>();
        }
    }
}