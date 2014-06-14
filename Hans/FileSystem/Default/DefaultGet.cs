using System.IO;

namespace Hans.FileSystem.Default
{
    public class DefaultGet : IGet
    {
        public string Combination(string part1, string part2)
        {
            return Path.Combine(part1, part2);
        }

        public string CombinationFullPath(string part1, string part2)
        {
            return FullPath(Combination(part1, part2));
        }

        public string FullPath(string path)
        {
            return Path.GetFullPath(path);
        }
    }
}