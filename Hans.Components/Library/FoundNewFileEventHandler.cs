using System;

namespace Hans.Components.Library
{
    /// <summary>
    /// The found new file event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FoundNewFileEventHandler(object sender, FoundNewFileEventArgs e);

    /// <summary>
    /// Event Arguments for the found new file event
    /// </summary>
    public class FoundNewFileEventArgs : EventArgs
    {
        /// <summary>
        /// The file that got found
        /// </summary>
        public string File { get; set; }
    }
}