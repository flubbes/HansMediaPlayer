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
        public void Save<T>(T toSave, string filePath)
        {
            var j = JsonConvert.SerializeObject(toSave);
            using (var s = File.CreateText(filePath))
            {
                s.Write(j);
            }
        }
    }
}
