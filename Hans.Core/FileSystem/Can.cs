namespace Hans.Core.FileSystem
{
    /// <summary>
    /// The Can class from the readability filesystem
    /// </summary>
    public class Can
    {
        private readonly IReadWrite _readWrite;

        /// <summary>
        /// Initializes a new instance of the can class
        /// </summary>
        /// <param name="readWrite"></param>
        public Can(IReadWrite readWrite)
        {
            _readWrite = readWrite;
        }

        /// <summary>
        /// The readwrite property
        /// </summary>
        public IReadWrite ReadWrite
        {
            get { return _readWrite; }
        }
    }
}