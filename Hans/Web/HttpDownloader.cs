using System;
using System.Net;

namespace Hans.Web
{
    public class HttpDownloader : IDownloader
    {
        public int Progress { get; private set; }

        public void Abort()
        {
            
        }

        public event EventHandler Failed;

        protected virtual void OnFailed()
        {
            var handler = Failed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void Start(DownloadRequest request)
        {
            if (request.Uri != null)
            {
                OnFailed();
                return;
            }
            var httpRequest = HttpWebRequest.CreateHttp(request.Uri);
            var webResponse = httpRequest.GetResponse();
            var responseStream = webResponse.GetResponseStream();
            var buffer = new byte[4096];
            var position = 0;
            while (responseStream.Read(buffer, position, buffer.Length) != 0)
            {
                
            }
        }
    }
}