using System.IO;

namespace Hans.FileSystem
{
    public interface IOpen
    {
        Stream ForWriting(string filePath);
    }
}