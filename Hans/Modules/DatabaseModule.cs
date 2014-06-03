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
    /// <summary>
    /// The database module that loads all components of the hans player
    /// </summary>
    public class DatabaseModule : NinjectModule
    {
        /// <summary>
        /// Loads all modules for the database
        /// </summary>
        public override void Load()
        {
            Bind<IDatabaseSaver>().To<FlatFileDatabaseSaver>();
        }
    }
}
