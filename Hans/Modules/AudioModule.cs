using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hans.Library;
using Ninject.Modules;

namespace Hans.Modules
{
    public class AudioModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAudioLoader>().To<DefaultAudioLoader>();
        }
    }
}
