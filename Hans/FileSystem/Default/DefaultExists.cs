namespace Hans.FileSystem.Default
{
    public class DefaultExists : IExists
    {
        public bool File(string path)
        {
            return System.IO.File.Exists(path);
        }
    }
}