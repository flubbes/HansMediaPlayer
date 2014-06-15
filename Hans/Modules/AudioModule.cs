using Hans.Audio;
using Hans.Library;
using Ninject.Modules;

namespace Hans.Modules
{
    /// <summary>
    /// The audio module of hans
    /// </summary>
    public class AudioModule : NinjectModule
    {
        /// <summary>
        /// Loads the module
        /// </summary>
        public override void Load()
        {
            Bind<IAudioLoader>().To<DefaultAudioLoader>();
            Bind<IAudioPlayer>().To<AudioPlayer>();
        }
    }
}