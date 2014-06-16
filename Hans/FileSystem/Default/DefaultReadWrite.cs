namespace Hans.FileSystem.Default
{
    /// <summary>
    /// The readwrite class from the readability filesystem
    /// </summary>
    public class DefaultReadWrite : IReadWrite
    {
        /// <summary>
        /// Determines whether the filesystem can read and write a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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