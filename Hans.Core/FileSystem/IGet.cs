namespace Hans.Core.FileSystem
{
    /// <summary>
    /// The get interface fromt he readability filesystem
    /// </summary>
    public interface IGet
    {
        /// <summary>
        /// Combines a path
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        string Combination(string part1, string part2);

        /// <summary>
        /// Combines and gets the full path
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        string CombinationFullPath(string part1, string part2);

        /// <summary>
        /// Gets the full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string FullPath(string path);
    }
}