using Hans.General;
using System;
using System.Collections.Generic;

namespace Hans.Web
{
    public class ThreadSafeList<T>
    {
        private object _lock = new object();
        private List<T> baseList;

        public ThreadSafeList()
        {
            baseList = new List<T>();
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return baseList.Count;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return baseList[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    baseList[index] = value;
                }
            }
        }

        public void Add(T item)
        {
            lock (_lock)
            {
                baseList.Add(item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                baseList.Clear();
            }
        }

        public IEnumerable<T> GetEnumerable()
        {
            lock (_lock)
            {
                return baseList.BuildThreadSafeCopy();
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return baseList.Remove(item);
            }
        }

        public void RemoveAt(int i)
        {
            lock (_lock)
            {
                baseList.RemoveAt(i);
            }
        }

        public void ThreadSafeForeach(Predicate<T> action)
        {
            for (int i = 0; i < baseList.Count; i++)
            {
                if (action.Invoke(this[i]))
                {
                    break;
                }
            }
        }
    }
}