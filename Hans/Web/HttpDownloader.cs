using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Hans.Web
{
    /// <summary>
    /// The http downloader
    /// </summary>
    public class HttpDownloader : IDownloader
    {
        private bool _abort;
        private Thread _thread;

        /// <summary>
        /// When the download failed
        /// </summary>
        public event EventHandler Failed;

        /// <summary>
        /// If the download is completed
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// If the current download is active
        /// </summary>
        public bool IsDownloading { get; private set; }

        /// <summary>
        /// The progress
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Aborts the download
        /// </summary>
        public void Abort()
        {
            _abort = true;
        }

        /// <summary>
        /// Starts the download
        /// </summary>
        /// <param name="request"></param>
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

        /// <summary>
        /// Triggers the fail event
        /// </summary>
        protected virtual void OnFailed()
        {
            var handler = Failed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
            IsDownloading = false;
        }

        /// <summary>
        /// Downloads a request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="absoluteFilePath"></param>
        private void Download(DownloadRequest request, string absoluteFilePath)
        {
            var fileSystem = request.FileSystem;
            using (var fileWriter = fileSystem.Open.ForWriting(absoluteFilePath))
            {
                DownloadFileToFileWriter(request, fileWriter);
            }
            Progress = 100;
        }

        /// <summary>
        /// Downloads a request to a filewriter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fileWriter"></param>
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

        /// <summary>
        /// Gets the reponse and writes to file (Yeah I know: A method should only do one thing!) AND (says it does two things) bla bla fuck you
        /// </summary>
        /// <param name="fileWriter"></param>
        /// <param name="httpWebRequest"></param>
        private void GetReponseAndWriteToFile(Stream fileWriter, HttpWebRequest httpWebRequest)
        {
            using (var webResponse = httpWebRequest.GetResponse())
            {
                GetRequestAndWriteToFile(fileWriter, webResponse);
            }
        }

        /// <summary>
        /// Gets the request and writes to file
        /// </summary>
        /// <param name="fileWriter"></param>
        /// <param name="webResponse"></param>
        private void GetRequestAndWriteToFile(Stream fileWriter, WebResponse webResponse)
        {
            var g = (int)webResponse.ContentLength;
            using (var responseStream = webResponse.GetResponseStream())
            {
                WriteWebStreamToFileStream(fileWriter, responseStream, g);
            }
        }

        /// <summary>
        /// Writes the web stream to the file stream
        /// </summary>
        /// <param name="fileWriter"></param>
        /// <param name="responseStream"></param>
        /// <param name="g"></param>
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