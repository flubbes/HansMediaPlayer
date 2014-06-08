using System.Collections;
using System.Collections.Generic;

namespace Hans.SongData
{
    public class DataFindMethodCollection : IEnumerable<IDataFindMethod>
    {
        private readonly IEnumerable<IDataFindMethod> _methods;

        public DataFindMethodCollection(IEnumerable<IDataFindMethod> methods)
        {
            _methods = methods;
        }

        public IEnumerator<IDataFindMethod> GetEnumerator()
        {
            return _methods.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}