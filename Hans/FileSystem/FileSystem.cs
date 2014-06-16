namespace Hans.FileSystem
{
    /// <summary>
    /// The readability filesystem
    /// </summary>
    public class FileSystem
    {
        private readonly Can _can;
        private readonly IExists _exists;
        private readonly IGet _get;
        private readonly IOpen _open;

        /// <summary>
        /// Initializes a new instance of the readability filesystem
        /// </summary>
        /// <param name="can"></param>
        /// <param name="open"></param>
        /// <param name="get"></param>
        /// <param name="exists"></param>
        public FileSystem(Can can, IOpen open, IGet get, IExists exists)
        {
            _can = can;
            _open = open;
            _get = get;
            _exists = exists;
        }

        /// <summary>
        /// If the filesystem can
        /// </summary>
        public Can Can
        {
            get { return _can; }
        }

        /// <summary>
        /// If something exists in the filesystem
        /// </summary>
        public IExists Exists
        {
            get { return _exists; }
        }

        /// <summary>
        /// Gets things from the filesystem
        /// </summary>
        public IGet Get
        {
            get { return _get; }
        }

        /// <summary>
        /// Opens things from the filesytem
        /// </summary>
        public IOpen Open
        {
            get { return _open; }
        }
    }
}