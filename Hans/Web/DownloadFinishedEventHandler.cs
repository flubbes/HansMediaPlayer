using System;

namespace Hans.Web
{
    public delegate void DownloadFinishedEventHandler(object sender, DownloadFinishedEventHandlerArgs e);

    public class DownloadFinishedEventHandlerArgs : EventArgs
    {
        public DownloadFinishedEventHandlerArgs(DownloadRequest downloadRequest)
        {
            DownloadRequest = downloadRequest;
        }

        public DownloadRequest DownloadRequest { get; set; }
    }
}