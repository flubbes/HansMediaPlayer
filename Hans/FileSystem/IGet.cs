namespace Hans.FileSystem
{
    public interface IGet
    {
        string Combination(string part1, string part2);

        string CombinationFullPath(string part1, string part2);

        string FullPath(string path);
    }
}