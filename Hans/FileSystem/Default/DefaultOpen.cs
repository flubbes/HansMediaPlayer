using System.IO;

namespace Hans.FileSystem.Default
{
    /// <summary>
    /// The open class from the readability filesystem
    /// </summary>
    public class DefaultOpen : IOpen
    {
        /// <summary>
        /// Opens a file for writing
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Stream ForWriting(string filePath)
        {
            return File.OpenWrite(filePath);
        }
    }
}