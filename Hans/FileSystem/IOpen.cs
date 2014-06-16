using System.IO;

namespace Hans.FileSystem
{
    /// <summary>
    /// The open interface from the readability filesystem
    /// </summary>
    public interface IOpen
    {
        /// <summary>
        /// For writing
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Stream ForWriting(string filePath);
    }
}