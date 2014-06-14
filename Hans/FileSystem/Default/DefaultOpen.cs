using System.IO;

namespace Hans.FileSystem.Default
{
    public class DefaultOpen : IOpen
    {
        public Stream ForWriting(string filePath)
        {
            return File.OpenWrite(filePath);
        }
    }
}