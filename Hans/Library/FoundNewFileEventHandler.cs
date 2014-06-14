using System;

namespace Hans.Library
{
    public delegate void FoundNewFileEventHandler(object sender, FoundNewFileEventArgs e);

    public class FoundNewFileEventArgs : EventArgs
    {
        public string File { get; set; }
    }
}