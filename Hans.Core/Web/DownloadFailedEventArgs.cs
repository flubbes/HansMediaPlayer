using System;

namespace Hans.Core.Web
{
    /// <summary>
    /// The event handler used for events when a download failed
    /// </summary>
    public class DownloadFailedEventArgs : EventArgs
    {
        /// <summary>
        /// The failed download request
        /// </summary>
        public DownloadRequest Request { get; set; }
    }
}