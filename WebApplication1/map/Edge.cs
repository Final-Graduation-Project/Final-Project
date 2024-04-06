using WebApplication1.map;

namespace WebApplication1.map

    
{
    public class Edge<T>
    {
        private Vertex<T> to;
        private double weight;

        public Edge(Vertex<T> endV, double inputWeight)
        {
            to = endV;
            weight = inputWeight;
        }

        public Vertex<T> GetTo()
        {
            return to;
        }

        public double GetWeight()
        {
            return weight;
        }
    }
}