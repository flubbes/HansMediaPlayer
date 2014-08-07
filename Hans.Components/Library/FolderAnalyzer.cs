using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Hans.Components.Library
{
    /// <summary>
    /// The folder analyzer
    /// </summary>
    public class FolderAnalyzer
    {
        private List<string> _resultFiles;

        /// <summary>
        /// Initializes a new folder analyzer
        /// </summary>
        public FolderAnalyzer()
        {
            _resultFiles = new List<string>();
        }

        /// <summary>
        /// When the analyzation is finished
        /// </summary>
        public event EventHandler AnalyzationFinished;

        /// <summary>
        /// When the analyzer found a new file
        /// </summary>
        public event FoundNewFileEventHandler FoundNewfile;

        /// <summary>
        /// FoundFiles counter
        /// </summary>
        public long FoundFiles { get; private set; }

        /// <summary>
        /// If the analyzer is currently running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// All the found files
        /// </summary>
        public IEnumerable<string> ResultFiles
        {
            get
            {
                lock (_resultFiles)
                {
                    return _resultFiles.AsEnumerable();
                }
            }
        }

        /// <summary>
        /// Searched folder counter
        /// </summary>
        public long SearchedFolders { get; private set; }

        /// <summary>
        /// Starts the analyter ansync
        /// </summary>
        /// <param name="analyzeFolderRequest"></param>
        public void StartAsync(AnalyzeFolderRequest analyzeFolderRequest)
        {
            if (IsRunning)
            {
                return;
            }
            StartSearchThread(analyzeFolderRequest);
            OnAnalyzationFinished();
        }

        /// <summary>
        /// Triggers the analyzation finished event
        /// </summary>
        protected virtual void OnAnalyzationFinished()
        {
            var handler = AnalyzationFinished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Triggers the found new file event
        /// </summary>
        /// <param name="file"></param>
        protected virtual void OnFoundNewfile(string file)
        {
            var handler = FoundNewfile;
            if (handler != null)
            {
                handler(this, new FoundNewFileEventArgs { File = file });
            }
        }

        /// <summary>
        /// Gets the top directories of a path
        /// </summary>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the top level files from a directory and filters with the given filter
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds files to the result files
        /// </summary>
        /// <param name="files"></param>
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

        /// <summary>
        /// Handles the analyze request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filters"></param>
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

        /// <summary>
        /// Analyzes all top level files with the given filter
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filters"></param>
        private void AnalyzeToplevelFilesWithFilter(string path, ref string[] filters)
        {
            foreach (var filter in filters)
            {
                var files = GetTopLevelFilesFromDirectoryWithFilter(path, filter);
                AddFilesToResultFiles(files);
            }
        }

        /// <summary>
        /// Searches the path recursive
        /// </summary>
        /// <param name="currentDirectory"></param>
        /// <param name="filters"></param>
        private void RecursiveSearch(string currentDirectory, ref string[] filters)
        {
            SearchedFolders++;
            var directories = GetTopDirectories(currentDirectory);
            AnalyzeToplevelFilesWithFilter(currentDirectory, ref filters);
            SearchFolders(ref filters, directories);
        }

        /// <summary>
        /// Searches folders
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="directories"></param>
        private void SearchFolders(ref string[] filters, string[] directories)
        {
            foreach (var directory in directories)
            {
                RecursiveSearch(directory, ref filters);
            }
        }

        /// <summary>
        /// Starts a search
        /// </summary>
        /// <param name="request"></param>
        private void StartSearch(AnalyzeFolderRequest request)
        {
            var filters = request.FileExtensionFilter.ToArray();
            IsRunning = true;
            AnalyzeRequest(request, filters);
            IsRunning = false;
        }

        /// <summary>
        /// Starts a new thread with a search
        /// </summary>
        /// <param name="analyzeFolderRequest"></param>
        private void StartSearchThread(AnalyzeFolderRequest analyzeFolderRequest)
        {
            new Thread(() => StartSearch(analyzeFolderRequest))
            {
                IsBackground = true
            }.Start();
        }
    }
}