namespace WebApplication1.map
{
    public class Distance
    {
        public string From { get; set; }
        public string To { get; set; }
        public double DistanceValue { get; set; } // Renamed the property to DistanceValue

        public Distance(string from, string to, double distance)
        {
            From = from;
            To = to;
            DistanceValue = distance;
        }
    }
}
