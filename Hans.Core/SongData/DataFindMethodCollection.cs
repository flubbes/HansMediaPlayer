using System.Collections;
using System.Collections.Generic;

namespace Hans.Core.SongData
{
    /// <summary>
    /// The collection with all methods to find data from a song
    /// </summary>
    public class DataFindMethodCollection : IEnumerable<IDataFindMethod>
    {
        private readonly IEnumerable<IDataFindMethod> _methods;

        /// <summary>
        /// Initializes a new instance of the dta find method collection
        /// </summary>
        /// <param name="methods"></param>
        public DataFindMethodCollection(IEnumerable<IDataFindMethod> methods)
        {
            _methods = methods;
        }

        /// <summary>
        /// Gets the collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDataFindMethod> GetEnumerator()
        {
            return _methods.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}