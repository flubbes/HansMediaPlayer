using Hans.Core.FileSystem;

namespace Hans.Components.FileSystem
{
    /// <summary>
    /// The exists class from the readability file system
    /// </summary>
    public class DefaultExists : IExists
    {
        /// <summary>
        /// Checks if a file exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool File(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}