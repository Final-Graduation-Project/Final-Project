namespace WebApplication1.Resorces
{
    public class EventResource
    {
        public string Name { get; set; }
        public int EventId { get; set; }

        public string Location { get; set; }
        public DateTime ExecutionTime { get; set; } // Ensure this is DateTime
        public DateTime Time { get; set; } // Ensure this is DateTime

        public string ResponsibleActivity { get; set; }
        public int StudentID { get; set; }
        public string ImagePath { get; set; } // Add if necessary

    }
}
