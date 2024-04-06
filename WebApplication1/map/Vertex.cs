using System;
using System.Collections.Generic;

namespace WebApplication1.map

{
    public class Vertex<T>
    {
        private T data;
        private LinkedList<Edge<T>> edges;

        public Vertex(T inputData)
        {
            data = inputData;
            edges = new LinkedList<Edge<T>>();
        }

        public void AddEdge(Vertex<T> endVertex, double weight)
        {
            edges.AddLast(new Edge<T>(endVertex, weight));
        }

        public void RemoveEdge(Vertex<T> endVertex)
        {
            Edge<T> edgeToRemove = null;
            foreach (var edge in edges)
            {
                if (edge.GetTo().Equals(endVertex))
                {
                    edgeToRemove = edge;
                    break;
                }
            }

            if (edgeToRemove != null)
            {
                edges.Remove(edgeToRemove);
            }
        }


        public T GetData()
        {
            return data;
        }

        public LinkedList<Edge<T>> GetEdges()
        {
            return edges;
        }

        public void Print(bool showWeight)
        {
            Console.Write(data + " --> ");

            if (edges.Count == 0)
            {
                Console.WriteLine();
                return;
            }

            foreach (var edge in edges)
            {
                Console.Write(edge.GetTo().GetData() + (showWeight ? $"({edge.GetWeight()})" : ""));
                if (edge != edges.Last.Value)
                    Console.Write(" --> ");
            }

            Console.WriteLine();
        }

        public override string ToString()
        {
            return data.ToString();
        }
    }
}
