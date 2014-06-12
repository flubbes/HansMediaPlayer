using Hans.SongData;
using Hans.SongData.DataFindMethods;
using NAudio.Codecs;
using Ninject.Modules;

namespace Hans.Modules
{
    public class SongDataModule : NinjectModule
    {
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
