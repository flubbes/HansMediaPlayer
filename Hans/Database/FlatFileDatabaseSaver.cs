using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hans.Properties;
using Newtonsoft.Json;

namespace Hans.Database
{
    public class FlatFileDatabaseSaver : IDatabaseSaver
    {
        public void Save<T>(T toSave)
        {
            var j = JsonConvert.SerializeObject(toSave);
            var databasePath = Settings.Default.Database_Path;
            using (var s = File.CreateText(databasePath))
            {
                s.Write(j);
            }
        }
    }
}
