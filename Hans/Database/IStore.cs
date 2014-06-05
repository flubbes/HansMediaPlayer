using System.Collections.Generic;

namespace Hans.Database
{
    public interface IStore<T>
    {
        void Add(T item);
        void Remove(T item);
        IEnumerable<T> GetEnumerable();
        void Update(T item);
    }
}