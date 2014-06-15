using Hans.General;
using System.IO;

namespace Hans.FileSystem.Default
{
    /// <summary>
    /// The get class from the readability filesystem
    /// </summary>
    public class DefaultGet : IGet
    {
        /// <summary>
        /// Combines a path
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        public string Combination(string part1, string part2)
        {
            return Path.Combine(part1.RemoveIllegalCharacters(), part2.RemoveIllegalCharacters());
        }

        /// <summary>
        /// Combines a path and gets the full path from the filesystem
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <returns></returns>
        public string CombinationFullPath(string part1, string part2)
        {
            return FullPath(Combination(part1, part2));
        }

        /// <summary>
        /// Gets the full path of the filesystem
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FullPath(string path)
        {
            return Path.GetFullPath(path);
        }
    }
}