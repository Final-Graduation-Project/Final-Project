using System.Collections.Generic;
using System;
namespace WebApplication1.map
{
    public class Graph<T>
    {
        private Dictionary<T, Vertex<T>> vertices;
        private bool isWeighted;
        private bool isDirected;

        public Graph()
        {
            vertices = new Dictionary<T, Vertex<T>>();
            isWeighted = false;
            isDirected = false;
        }

        public Graph(bool isWeighted, bool isDirected)
        {
            vertices = new Dictionary<T, Vertex<T>>();
            this.isWeighted = isWeighted;
            this.isDirected = isDirected;
        }

        public Vertex<T> AddVertex(T data)
        {
            var newVertex = new Vertex<T>(data);
            vertices[data] = newVertex;
            return newVertex;
        }

        public void AddEdge(Vertex<T> vertex1, Vertex<T> vertex2, double weight)
        {
            if (!isWeighted)
                weight = 0;

            vertex1.AddEdge(vertex2, weight);

            if (!isDirected)
                vertex2.AddEdge(vertex1, weight);
        }
        public void RemoveEdge(Vertex<T> vertex1, Vertex<T> vertex2)
        {
            vertex1.RemoveEdge(vertex2);

            if (!isDirected)
                vertex2.RemoveEdge(vertex1);
        }

        public void RemoveVertex(Vertex<T> vertex)
        {
            foreach (var v in vertices.Values)
            {
                v.RemoveEdge(vertex);
            }

            vertices.Remove(vertex.GetData());
        }

        public Dictionary<T, Vertex<T>> GetVertices()
        {
            return vertices;
        }

        public bool IsWeighted()
        {
            return isWeighted;
        }

        public bool IsDirected()
        {
            return isDirected;
        }

        public Vertex<T> GetVertex(T data)
        {
            return vertices.GetValueOrDefault(data);
        }
    }
}





