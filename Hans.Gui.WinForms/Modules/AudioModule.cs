using Hans.Components.Audio;
using Hans.Core.Audio;
using Ninject.Modules;

namespace Hans.Gui.WinForms.Modules
{
    public class AudioModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IAudioPlayer>().To<AudioPlayer>();
            Kernel.Bind<IAudioLoader>().To<DefaultAudioLoader>();
        }
    }
}
