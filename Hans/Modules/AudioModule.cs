using Hans.General;
using Hans.Library;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Modules
{
    public class AudioModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAudioLoader>().To<DefaultAudioLoader>();
            Bind<IAudioPlayer>().To<AudioPlayer>();
        }
    }
}