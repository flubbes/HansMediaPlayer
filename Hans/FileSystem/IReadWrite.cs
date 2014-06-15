namespace Hans.FileSystem
{
    public interface IReadWrite
    {
        /// <summary>
        /// Checks if the filesystem can read an write a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool File(string path);
    }
}