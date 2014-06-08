using Hans.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hans.SongData
{
    public class SongDataFinder : ISongDataFinder
    {
        private readonly DataFindMethodCollection _dataFindMethod;
        private readonly List<FindSongDataRequest> _requests;

        public SongDataFinder(DataFindMethodCollection dataFindMethod)
        {
            _dataFindMethod = dataFindMethod;
            _requests = new List<FindSongDataRequest>();
            new Thread(DataFinderThreadMethod) { IsBackground = true }.Start();
        }

        public event FoundDataEventHandler FoundData;

        public void FindAsync(FindSongDataRequest request)
        {
            _requests.Add(request);
        }

        protected virtual void OnFoundData(SongDataResponse songdata)
        {
            var handler = FoundData;
            if (handler != null)
            {
                handler(songdata);
            }
        }

        private void DataFinderThreadMethod()
        {
            while (true)
            {
                if (HasRequests())
                {
                    var request = PeekRequest();
                    foreach (var m in _dataFindMethod)
                    {
                        var d = m.GetData(request);
                    }
                    _requests.RemoveAt(0);
                }
                Thread.Sleep(1);
            }
        }

        private bool HasRequests()
        {
            return _requests.Count > 0;
        }

        private FindSongDataRequest PeekRequest()
        {
            return _requests.ElementAt(0);
        }
    }
}