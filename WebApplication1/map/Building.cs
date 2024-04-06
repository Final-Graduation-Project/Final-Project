namespace WebApplication1.map
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Building(string name,double latitude,double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
    
}
