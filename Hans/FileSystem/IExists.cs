namespace Hans.FileSystem
{
    /// <summary>
    /// The exists interface from the filesystem
    /// </summary>
    public interface IExists
    {
        /// <summary>
        /// Determines whether a file exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool File(string path);
    }
}