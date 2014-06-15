using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Hans.Web
{
    public class HttpDownloader : IDownloader
    {
        private bool _abort;
        private Thread _thread;

        public event EventHandler Failed;

        public bool IsComplete { get; private set; }

        public bool IsDownloading { get; private set; }

        public int Progress { get; private set; }

        public void Abort()
        {
            _abort = true;
        }

        public void Start(DownloadRequest request)
        {
            IsDownloading = true;
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
            IsDownloading = false;
        }

        private void Download(DownloadRequest request, string absoluteFilePath)
        {
            var fileSystem = request.FileSystem;
            using (var fileWriter = fileSystem.Open.ForWriting(absoluteFilePath))
            {
                DownloadFileToFileWriter(request, fileWriter);
            }
            Progress = 100;
        }

        private void DownloadFileToFileWriter(DownloadRequest request, Stream fileWriter)
        {
            var httpWebRequest = WebRequest.CreateHttp(request.Uri);
            try
            {
                GetReponseAndWriteToFile(fileWriter, httpWebRequest);
            }
            catch
            {
                OnFailed();
            }
        }

        private void GetReponseAndWriteToFile(Stream fileWriter, HttpWebRequest httpWebRequest)
        {
            using (var webResponse = httpWebRequest.GetResponse())
            {
                GetRequestAndWriteToFile(fileWriter, webResponse);
            }
        }

        private void GetRequestAndWriteToFile(Stream fileWriter, WebResponse webResponse)
        {
            var g = (int)webResponse.ContentLength;
            using (var responseStream = webResponse.GetResponseStream())
            {
                WriteWebStreamToFileStream(fileWriter, responseStream, g);
            }
        }

        private void WriteWebStreamToFileStream(Stream fileWriter, Stream responseStream, int g)
        {
            var buffer = new byte[4096];
            var written = 0;
            var writtenTotal = 0;
            do
            {
                written = responseStream.Read(buffer, 0, buffer.Length);
                fileWriter.Write(buffer, 0, written);
                writtenTotal += written;
                Progress = writtenTotal * 100 / g;
            } while (written > 0 && !_abort);
            IsDownloading = false;
            IsComplete = true;
        }
    }
}