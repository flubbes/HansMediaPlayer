namespace Hans.FileSystem
{
    public class Can
    {
        private readonly IReadWrite _readWrite;

        public Can(IReadWrite readWrite)
        {
            _readWrite = readWrite;
        }

        public IReadWrite ReadWrite
        {
            get { return _readWrite; }
        }
    }
}