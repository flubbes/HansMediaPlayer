using Hans.Core.SongData;
using Hans.Core.SongData.DataFindMethods;
using Ninject.Modules;

namespace Hans.Modules
{
    /// <summary>
    /// The songdata module for hans
    /// </summary>
    public class SongDataModule : NinjectModule
    {
        /// <summary>
        /// Loads the module
        /// </summary>
        public override void Load()
        {
            Bind<ISongDataFinder>().To<SongDataFinder>();
            Bind<DataFindMethodCollection>().ToMethod(context => new DataFindMethodCollection(new IDataFindMethod[]
            {
                new Id3TagDataFindMethod(),
                new FileNameDataFindMethod(), 
            }));
        }
    }
}