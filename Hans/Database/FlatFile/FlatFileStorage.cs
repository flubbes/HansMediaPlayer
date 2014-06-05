using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Ninject.Infrastructure.Language;

namespace Hans.Database.FlatFile
{
    public class FlatFileStorage<T> : IStore<T>
    {
        private readonly string _path;
        private readonly List<T> _cache;
        private readonly object _threadLock;
        private DateTime _lastCacheUpdate;

        public FlatFileStorage(string path)
        {
            _path = path;
            _threadLock = new object();
            _cache = new List<T>(GetCacheFromFile());
        }

        public void Add(T item)
        {
            lock (_cache)
            {
                _cache.Add(item);
            }
            _lastCacheUpdate = DateTime.Now;
            CommitCachedChangesAsync();
        }

        public void Remove(T item)
        {
            lock (_cache)
            {
                _cache.Remove(item);
            }
            _lastCacheUpdate = DateTime.Now;
            CommitCachedChangesAsync();
        }

        public IEnumerable<T> GetEnumerable()
        {
            lock (_cache)
            {
                return _cache.ToEnumerable();
            }
        }

        public void Update(T item)
        {
            lock (_cache)
            {
                var index = _cache.FindIndex(i => i.Equals(item));
                _cache[index] = item;
            }
            _lastCacheUpdate = DateTime.Now;
            CommitCachedChangesAsync();
        }

        /// <summary>
        /// gets the file stream reader
        /// </summary>
        /// <returns></returns>
        private StreamReader GetFileStreamReader()
        {
            return File.OpenText(_path);
        }

        /// <summary>
        /// Gets the content of a file
        /// </summary>
        /// <returns></returns>
        private string GetFileContent()
        {
            using (var fs = GetFileStreamReader())
            {
                return fs.ReadToEnd();
            }
        }

        /// <summary>
        /// builds the hansplaylists from the json string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static IEnumerable<T> BuildCacheFromJson(string json)
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }

        /// <summary>
        /// Gets all playlists from the file
        /// </summary>
        /// <returns></returns>
        private IEnumerable<T> GetCacheFromFile()
        {
            return BuildCacheFromJson(GetFileContent());
        }

        /// <summary>
        /// Gets the file stream writer 
        /// </summary>
        /// <returns></returns>
        private StreamWriter GetFileStreamWriter()
        {
            return File.CreateText(_path);
        }


        /// <summary>
        /// Writes the chache to the file
        /// </summary>
        private void WriteCacheToFile()
        {
            lock (_cache)
            {
                using (var fw = GetFileStreamWriter())
                {
                    fw.Write(JsonConvert.SerializeObject(_cache));
                }
                _lastCacheUpdate = DateTime.Now;
            }
        }

        /// <summary>
        /// Get the time since the last changed
        /// </summary>
        /// <returns></returns>
        private TimeSpan TimeSinceLastCacheChange()
        {
            return (DateTime.Now - _lastCacheUpdate);
        }


        /// <summary>
        /// Commites all changed made since the last commit to the file asynchronious
        /// </summary>
        private void CommitCachedChangesAsync()
        {
            if (ThreadIsNotLocked())
            {
                new Thread(CommitCachedChanges).Start();
            }
        }

        /// <summary>
        /// Checks whether the thread is locked
        /// </summary>
        /// <returns></returns>
        private bool ThreadIsNotLocked()
        {
            return Monitor.TryEnter(_threadLock);
        }

        /// <summary>
        /// Commites all changed made since the last commit to the file
        /// </summary>
        private void CommitCachedChanges()
        {
            lock (_threadLock)
            {
                WaitTillNothingHasChangedForTenSeconds();
                WriteCacheToFile();
            }
        }

        /// <summary>
        /// Waits till nothing has changed for ten seconds
        /// </summary>
        private void WaitTillNothingHasChangedForTenSeconds()
        {
            while (TimeSinceLastCacheChange().TotalSeconds <= 10)
            {
                Thread.Sleep(10);
            }
        }
    }
}