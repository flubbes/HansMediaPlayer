using System.Collections.Generic;

namespace Hans.Database
{
    /// <summary>
    /// The base store interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStore<T>
    {
        /// <summary>
        /// Adds an item to the store
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Gets all items in the store
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetEnumerable();

        /// <summary>
        /// Removes an item from the store
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);

        /// <summary>
        /// Updates an item fomr the store
        /// </summary>
        /// <param name="item"></param>
        void Update(T item);
    }
}