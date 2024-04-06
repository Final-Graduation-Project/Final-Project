using System;
using System.Collections.Generic;
namespace WebApplication1.map
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> data;

        public PriorityQueue()
        {
            data = new List<T>();
        }

        public int Count => data.Count;

        public void Enqueue(T item)
        {
            data.Add(item);
            int ci = data.Count - 1;
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if (data[ci].CompareTo(data[pi]) >= 0)
                    break;
                T temp = data[ci];
                data[ci] = data[pi];
                data[pi] = temp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            if (data.Count == 0)
                throw new InvalidOperationException("Priority queue is empty");

            T frontItem = data[0];
            int lastIndex = data.Count - 1;
            data[0] = data[lastIndex];
            data.RemoveAt(lastIndex);

            lastIndex--;
            int pi = 0;
            while (true)
            {
                int ci = pi * 2 + 1;
                if (ci > lastIndex) break;
                int rc = ci + 1;
                if (rc <= lastIndex && data[rc].CompareTo(data[ci]) < 0)
                    ci = rc;
                if (data[pi].CompareTo(data[ci]) <= 0)
                    break;
                T temp = data[pi];
                data[pi] = data[ci];
                data[ci] = temp;
                pi = ci;
            }

            return frontItem;
        }
    }
}