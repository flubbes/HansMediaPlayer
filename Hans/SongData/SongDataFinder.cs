using Hans.General;
using Hans.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hans.SongData
{
    /// <summary>
    /// The default song data finder from hans
    /// </summary>
    public class SongDataFinder : ISongDataFinder
    {
        public const char Splitter = '◘';
        private readonly DataFindMethodCollection _dataFindMethod;
        private readonly List<FindSongDataRequest> _requests;
        private bool _exit;

        /// <summary>
        /// Initializes a new instance of the song data finder from hans
        /// </summary>
        /// <param name="dataFindMethod"></param>
        /// <param name="exitAppTrigger"></param>
        public SongDataFinder(DataFindMethodCollection dataFindMethod, ExitAppTrigger exitAppTrigger)
        {
            exitAppTrigger.GotTriggered += exitAppTrigger_GotTriggered;
            _dataFindMethod = dataFindMethod;
            _requests = new List<FindSongDataRequest>();
            new Thread(DataFinderThreadMethod) { IsBackground = true }.Start();
        }

        /// <summary>
        /// When it found data
        /// </summary>
        public event FoundDataEventHandler FoundData;

        /// <summary>
        /// Starts the data searcher
        /// </summary>
        /// <param name="request"></param>
        public void FindAsync(FindSongDataRequest request)
        {
            lock (_requests)
            {
                _requests.Add(request);
            }
        }

        /// <summary>
        /// If he found new data
        /// </summary>
        /// <param name="songdata"></param>
        protected virtual void OnFoundData(SongDataResponse songdata)
        {
            var handler = FoundData;
            if (handler != null)
            {
                handler(this, new FoundDataEventArgs { SongData = songdata });
            }
        }

        /// <summary>
        /// Extracts a list from a string with the given splitter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        private static List<string> ExtractList(string value, char splitter)
        {
            var splitArr = value.Split(splitter);
            return new List<string>(splitArr);
        }

        /// <summary>
        /// The method for the thread
        /// </summary>
        private void DataFinderThreadMethod()
        {
            while (!_exit)
            {
                while (HasRequests())
                {
                    var request = DequeueRequest();
                    var data = new Dictionary<string, string>();
                    foreach (var m in _dataFindMethod)
                    {
                        MergeDictionaries(m.GetData(request), data);
                    }
                    var songDataResponse = new SongDataResponse
                    {
                        FilePath = request.PathToFile,
                        SongId = request.SongId,
                        Artists = ExtractStringList(data, "Artists", Splitter),
                        Title = ExtractString(data, "Title"),
                        Genre = ExtractString(data, "Genre"),
                    };
                    OnFoundData(songDataResponse);
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Deuques a request
        /// </summary>
        /// <returns></returns>
        private FindSongDataRequest DequeueRequest()
        {
            lock (_requests)
            {
                var result = _requests.ElementAt(0);
                _requests.RemoveAt(0);
                return result;
            }
        }

        /// <summary>
        /// Gets called when the exit app trigger got triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void exitAppTrigger_GotTriggered(object sender, EventArgs eventArgs)
        {
            _exit = true;
        }

        /// <summary>
        /// Extracts a stirng from a dictionary
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string ExtractString(Dictionary<string, string> data, string key)
        {
            return data.ContainsKey(key) ? data[key] : string.Empty;
        }

        /// <summary>
        /// Extracts a list from a dictionary item with the given splitter
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        private List<string> ExtractStringList(Dictionary<string, string> data, string key, char splitter)
        {
            if (!data.ContainsKey(key))
            {
                return new List<string>();
            }
            var value = data[key];
            return value.Contains(splitter) ? ExtractList(value, splitter) : new List<string>(new[] { value });
        }

        /// <summary>
        /// If there are more requests
        /// </summary>
        /// <returns></returns>
        private bool HasRequests()
        {
            lock (_requests)
            {
                return _requests.Count > 0;
            }
        }

        /// <summary>
        /// Merges two dictionaries
        /// </summary>
        /// <param name="toMerge"></param>
        /// <param name="mergeInto"></param>
        private void MergeDictionaries(Dictionary<string, string> toMerge, Dictionary<string, string> mergeInto)
        {
            foreach (var kvp in toMerge)
            {
                if (!mergeInto.ContainsKey(kvp.Key))
                {
                    mergeInto.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    //TODO merge qualifier
                }
            }
        }
    }
}