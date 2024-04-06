using System;

namespace WebApplication1.map
{
    public class QueueObject<T> : IComparable<QueueObject<T>>
    {
        public Vertex<T> Vertex { get; private set; }
        public double Priority { get; private set; }

        public QueueObject(Vertex<T> vertex, double priority)
        {
            Vertex = vertex;
            Priority = priority;
        }

        public int CompareTo(QueueObject<T> other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}
