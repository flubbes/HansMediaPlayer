using System;

namespace Hans
{
    public class HansSong
    {

        public HansSong(string path)
        {
            Path = path;
        }

        public long Length
        {
            get { return 0; }
        }

        public long Position
        {
            get { return 0; }
        }

        public String Path { get; set; }
    }
}