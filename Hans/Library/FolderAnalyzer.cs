using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Hans.Library
{
    public class FolderAnalyzer
    {
        private List<string> _resultFiles;

        public FolderAnalyzer()
        {
            _resultFiles = new List<string>();
        }

        public event EventHandler AnalyzationFinished;

        public event FoundNewFileEventHandler FoundNewfile;

        public long FoundFiles { get; private set; }

        public bool IsRunning { get; private set; }

        public IEnumerable<string> ResultFiles
        {
            get
            {
                lock (_resultFiles)
                {
                    return _resultFiles.ToEnumerable();
                }
            }
        }

        public long SearchedFolders { get; private set; }

        public void StartAsync(AnalyzeFolderRequest analyzeFolderRequest)
        {
            if (IsRunning)
            {
                return;
            }
            StartSearchThread(analyzeFolderRequest);
            OnAnalyzationFinished();
        }

        protected virtual void OnAnalyzationFinished()
        {
            var handler = AnalyzationFinished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnFoundNewfile(string file)
        {
            var handler = FoundNewfile;
            if (handler != null)
            {
                handler(this, new FoundNewFileEventArgs { File = file });
            }
        }

        private static string[] GetTopDirectories(string currentDirectory)
        {
            try
            {
                return Directory.GetDirectories(currentDirectory, "*",
                SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        private static string[] GetTopLevelFilesFromDirectoryWithFilter(string path, string filter)
        {
            try
            {
                return Directory.GetFiles(path, filter, SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        private void AddFilesToResultFiles(string[] files)
        {
            lock (_resultFiles)
            {
                foreach (var file in files)
                {
                    _resultFiles.Add(file);
                    OnFoundNewfile(file);
                }
            }
            FoundFiles += files.Count();
        }

        private void AnalyzeRequest(AnalyzeFolderRequest request, string[] filters)
        {
            if (request.ShouldAnalyzeSubDirectories)
            {
                RecursiveSearch(request.Path, ref filters);
            }
            else
            {
                AnalyzeToplevelFilesWithFilter(request.Path, ref filters);
            }
        }

        private void AnalyzeToplevelFilesWithFilter(string path, ref string[] filters)
        {
            foreach (var filter in filters)
            {
                var files = GetTopLevelFilesFromDirectoryWithFilter(path, filter);
                AddFilesToResultFiles(files);
            }
        }

        private void RecursiveSearch(string currentDirectory, ref string[] filters)
        {
            SearchedFolders++;
            var directories = GetTopDirectories(currentDirectory);
            AnalyzeToplevelFilesWithFilter(currentDirectory, ref filters);
            SearchFolders(ref filters, directories);
        }

        private void SearchFolders(ref string[] filters, string[] directories)
        {
            foreach (var directory in directories)
            {
                RecursiveSearch(directory, ref filters);
            }
        }

        private void StartSearch(AnalyzeFolderRequest request)
        {
            var filters = request.FileExtensionFilter.ToArray();
            IsRunning = true;
            AnalyzeRequest(request, filters);
            IsRunning = false;
        }

        private void StartSearchThread(AnalyzeFolderRequest analyzeFolderRequest)
        {
            new Thread(() => StartSearch(analyzeFolderRequest))
            {
                IsBackground = true
            }.Start();
        }
    }
}