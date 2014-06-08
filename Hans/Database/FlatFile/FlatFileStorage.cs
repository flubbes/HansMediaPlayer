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
        private bool _cacheChanged;

        public FlatFileStorage(string path)
        {
            _path = path;
            _threadLock = new object();
            _cache = new List<T>(GetCacheFromFile() ?? new T[0]);
            new Thread(CommitThread) {IsBackground = true}.Start();
        }

        private void CommitThread()
        {
            while (true)
            {
                CommitCachedChanges();
                Thread.Sleep(1000);
            }
        }

        public void Add(T item)
        {
            lock (_cache)
            {
                _cache.Add(item);
            }
            CacheUpdate();
        }

        private void CacheUpdate()
        {
            _lastCacheUpdate = DateTime.Now;
            _cacheChanged = true;
        }

        public void Remove(T item)
        {
            lock (_cache)
            {
                _cache.Remove(item);
            }
            CacheUpdate();
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
            CacheUpdate();
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
            return DatabaseFileDoesNotExist() ? string.Empty : ReadFileContent();
        }

        private bool DatabaseFileDoesNotExist()
        {
            return !File.Exists(_path);
        }

        private string ReadFileContent()
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
                    fw.Write(JsonConvert.SerializeObject(_cache, Formatting.Indented));
                }
                _cacheChanged = false;
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
        /// Commites all changed made since the last commit to the file
        /// </summary>
        private void CommitCachedChanges()
        {
            if (TimeSinceLastCacheChange().TotalSeconds >= 10 && _cacheChanged)
            {
                WriteCacheToFile();
            }
        }
    }
}