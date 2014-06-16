using Hans.SongData;
using Hans.SongData.DataFindMethods;
using Ninject.Modules;

namespace Hans.Modules
{
    /// <summary>
    /// The song data module for hans
    /// </summary>
    public class SongDataModule : NinjectModule
    {
        /// <summary>
        /// Loads the module
        /// </summary>
        public override void Load()
        {
            Bind<ISongDataFinder>().To<SongDataFinder>();
            Bind<DataFindMethodCollection>().ToMethod(context => new DataFindMethodCollection(new[]
            {
                new Id3TagDataFindMethod()
            }));
        }
    }
}