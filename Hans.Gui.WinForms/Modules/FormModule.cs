using Hans.Gui.WinForms.Data;
using Ninject.Modules;

namespace Hans.Gui.WinForms.Modules
{
    public class FormModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISongListViewFiller>().To<SongListViewFiller>();
        }
    }
}
