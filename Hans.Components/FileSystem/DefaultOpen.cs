using System.IO;
using Hans.Core.FileSystem;

namespace Hans.Components.FileSystem
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