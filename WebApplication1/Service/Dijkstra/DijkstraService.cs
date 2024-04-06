using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Services.Dijkstra
{
    public interface IDijkstraService
    {
        Dictionary<string, Tuple<double, List<string>>> CalculateShortestPath(string startNode, string endNode);
    }

    public class DijkstraService : IDijkstraService
    {
        private readonly Dictionary<string, Dictionary<string, double>> _graph;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DijkstraService(Dictionary<string, Dictionary<string, double>> graph, IHttpContextAccessor httpContextAccessor)
        {
            _graph = graph;
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<string, Tuple<double, List<string>>> CalculateShortestPath(string startNode, string endNode)
        {
            var visited = new HashSet<string>();
            var distances = new Dictionary<string, double>();
            var paths = new Dictionary<string, List<string>>();
            var queue = new Queue<string>();

            foreach (var node in _graph.Keys)
            {
                distances[node] = double.PositiveInfinity;
                paths[node] = new List<string>();
            }

            distances[startNode] = 0;
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                if (visited.Contains(currentNode))
                    continue;

                visited.Add(currentNode);

                foreach (var neighbor in _graph[currentNode])
                {
                    var neighborNode = neighbor.Key;
                    var neighborDistance = neighbor.Value;

                    if (!visited.Contains(neighborNode))
                    {
                        var totalDistance = distances[currentNode] + neighborDistance;

                        if (totalDistance < distances[neighborNode])
                        {
                            distances[neighborNode] = totalDistance;
                            paths[neighborNode] = new List<string>(paths[currentNode]);
                            paths[neighborNode].Add(neighborNode);
                            queue.Enqueue(neighborNode);
                        }
                    }
                }
            }

            var result = new Dictionary<string, Tuple<double, List<string>>>();
            foreach (var node in _graph.Keys)
            {
                result[node] = new Tuple<double, List<string>>(distances[node], paths[node]);
            }

            return result;
        }
    }
}
