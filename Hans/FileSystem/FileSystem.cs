using System.ComponentModel;
using System.IO;

namespace Hans.FileSystem
{
    public class FileSystem
    {
        private readonly Can _can;
        private readonly IExists _exists;
        private readonly IGet _get;
        private readonly IOpen _open;

        public FileSystem(Can can, IOpen open, IGet get, IExists exists)
        {
            _can = can;
            _open = open;
            _get = get;
            _exists = exists;
        }

        public Can Can
        {
            get { return _can; }
        }

        public IExists Exists
        {
            get { return _exists; }
        }

        public IGet Get
        {
            get { return _get; }
        }

        public IOpen Open
        {
            get { return _open; }
        }
    }
}