using System;
using System.Net;
using System.Threading;

namespace Hans.Web
{
    public class HttpDownloader : IDownloader
    {
        private bool _abort;
        private Thread _thread;

        public event EventHandler Failed;

        public int Progress { get; private set; }

        public void Abort()
        {
            _thread.Abort();
        }

        public void Start(DownloadRequest request)
        {
            //TODO clean code
            var fileSystem = request.FileSystem;
            var filePath = request.FileSystem.Get.CombinationFullPath(request.DestinationDirectory, request.FileName);
            if (request.Uri == null
                || fileSystem.Exists.File(filePath) && !fileSystem.Can.ReadWrite.File(filePath))
            {
                OnFailed();
                return;
            }
            _thread = new Thread(() => Download(request, filePath)) { IsBackground = true };
            _thread.Start();
        }

        protected virtual void OnFailed()
        {
            var handler = Failed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private bool CanAccessFile(string filePath)
        {
            throw new NotImplementedException();
        }

        private void Download(DownloadRequest request, string absoluteFilePath)
        {
            //TODO clean code
            var fileSystem = request.FileSystem;
            using (var fileWriter = fileSystem.Open.ForWriting(absoluteFilePath))
            {
                using (var webResponse = WebRequest.CreateHttp(request.Uri).GetResponse())
                {
                    var g = (int)webResponse.ContentLength;
                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        var buffer = new byte[4096];
                        var written = 0;
                        do
                        {
                            written = responseStream.Read(buffer, 0, buffer.Length);
                            fileWriter.Write(buffer, 0, written);
                            Progress = written * 100 / g;
                        } while (written > 0 && !_abort);
                    }
                }
            }
            Progress = 100;
        }
    }
}