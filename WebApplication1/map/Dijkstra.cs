using System;
using System.Collections.Generic;
namespace WebApplication1.map
{
    public class Dijkstra<T>
    {
        private Graph<T> graph;
        private Dictionary<T, double> distances;
        private Dictionary<T, Vertex<T>> previous;

        public Dijkstra(Graph<T> g)
        {
            if (g == null)
                throw new ArgumentNullException("Graph cannot be null");
            if (!g.IsWeighted())
                throw new ArgumentException("Graph must be weighted");

            distances = new Dictionary<T, double>();
            previous = new Dictionary<T, Vertex<T>>();
            graph = g;
        }

        private void DijkstraAlgo(Vertex<T> start)
        {
            distances.Clear();
            previous.Clear();

            var queue = new PriorityQueue<QueueObject<T>>();

            queue.Enqueue(new QueueObject<T>(start, 0.0));

            foreach (var vertex in graph.GetVertices().Values)
            {
                distances[vertex.GetData()] = double.PositiveInfinity;
                previous[vertex.GetData()] = null;
            }

            distances[start.GetData()] = 0.0;

            while (queue.Count > 0)
            {
                var v = queue.Dequeue().Vertex;

                foreach (var e in v.GetEdges())
                {
                    var alt = distances[v.GetData()] + e.GetWeight();
                    var to = e.GetTo();

                    if (alt < distances[to.GetData()])
                    {
                        distances[to.GetData()] = alt;
                        previous[to.GetData()] = v;
                        queue.Enqueue(new QueueObject<T>(to, distances[to.GetData()]));
                    }
                }
            }
        }

        public List<Vertex<T>> GetShortestPath(Vertex<T> start, Vertex<T> end)
        {
            if (start == null || end == null)
                throw new ArgumentException("Start and end vertices cannot be null");
            if (start.Equals(end))
                throw new ArgumentException("Start and end vertices cannot be the same");

            DijkstraAlgo(start);

            var path = new List<Vertex<T>>();
            var current = end;

            while (current != null)
            {
                path.Insert(0, current);
                current = previous[current.GetData()];
            }

            return path[0].Equals(start) ? path : null;
        }
    }
}

