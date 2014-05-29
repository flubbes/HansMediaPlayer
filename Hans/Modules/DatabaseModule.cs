using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hans.Database;
using Ninject;
using Ninject.Modules;

namespace Hans.Modules
{
    public class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseSaver>().To<FlatFileDatabaseSaver>();
        }
    }
}
