using System;

namespace Hans.FileSystem.Default
{
    public class DefaultReadWrite : IReadWrite
    {
        public bool File(string path)
        {
            try
            {
                using (var fileStream = System.IO.File.OpenWrite(path))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}