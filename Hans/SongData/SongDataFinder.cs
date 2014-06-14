using Hans.General;
using Hans.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hans.SongData
{
    public class SongDataFinder : ISongDataFinder
    {
        public const char Splitter = '◘';
        private readonly DataFindMethodCollection _dataFindMethod;
        private readonly List<FindSongDataRequest> _requests;
        private bool _exit;

        public SongDataFinder(DataFindMethodCollection dataFindMethod, ExitAppTrigger exitAppTrigger)
        {
            exitAppTrigger.GotTriggered += exitAppTrigger_GotTriggered;
            _dataFindMethod = dataFindMethod;
            _requests = new List<FindSongDataRequest>();
            new Thread(DataFinderThreadMethod) { IsBackground = true }.Start();
        }

        public event FoundDataEventHandler FoundData;

        public void FindAsync(FindSongDataRequest request)
        {
            lock (_requests)
            {
                _requests.Add(request);
            }
        }

        protected virtual void OnFoundData(SongDataResponse songdata)
        {
            var handler = FoundData;
            if (handler != null)
            {
                handler(this, new FoundDataEventArgs { SongData = songdata });
            }
        }

        private static List<string> ExtractList(string value, char splitter)
        {
            var splitArr = value.Split(splitter);
            return new List<string>(splitArr);
        }

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
                        Genre = ExtractString(data, "Genre")
                    };
                    OnFoundData(songDataResponse);
                }
                Thread.Sleep(1);
            }
        }

        private FindSongDataRequest DequeueRequest()
        {
            lock (_requests)
            {
                var result = _requests.ElementAt(0);
                _requests.RemoveAt(0);
                return result;
            }
        }

        private void exitAppTrigger_GotTriggered(object sender, EventArgs eventArgs)
        {
            _exit = true;
        }

        private string ExtractString(Dictionary<string, string> data, string key)
        {
            return data.ContainsKey(key) ? data[key] : string.Empty;
        }

        private List<string> ExtractStringList(Dictionary<string, string> data, string key, char splitter)
        {
            if (!data.ContainsKey(key))
            {
                return new List<string>();
            }
            var value = data[key];
            return value.Contains(splitter) ? ExtractList(value, splitter) : new List<string>(new[] { value });
        }

        private bool HasRequests()
        {
            lock (_requests)
            {
                return _requests.Count > 0;
            }
        }

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