namespace WebApplication1.Models
{
    public class EventAddEntitycs
    {
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string LocationOfActivity { get; set; }
        public DateTime ActivityExecutionTime { get; set; }
        public DateTime Time { get; set; } // Ensure this is DateTime
        public string EntityResponsibleActivity { get; set; }
        public int ConcilMemberID { get; set; }
        public string ImagePath { get; set; } // Add if necessary
    }
}
