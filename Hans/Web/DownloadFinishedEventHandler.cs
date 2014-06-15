using System;

namespace Hans.Web
{
    /// <summary>
    /// The download finished event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DownloadFinishedEventHandler(object sender, DownloadFinishedEventArgs e);

    /// <summary>
    /// The download finished event args
    /// </summary>
    public class DownloadFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// The download request that got finished
        /// </summary>
        public DownloadRequest DownloadRequest { get; set; }
    }
}