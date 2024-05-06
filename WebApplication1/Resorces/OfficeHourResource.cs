namespace WebApplication1.Resorces
{
    public class OfficeHourResource
    {
        public int OfficeHourid { get; set; }
        public int teacherid { get; set; }
        public DateTime tehcherFreeDay { get; set; }

        public DateTime tehcerstartFreeTime { get; set; }
        public DateTime tehcerEndFreeTime { get; set; }
        public string buildingName { get; set; }
        public string rommNumber { get; set; }
    }
}
