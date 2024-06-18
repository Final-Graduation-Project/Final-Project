using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Table
{
    public class EventEntity
    {
        [Key]
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string LocationOfActivity { get; set; }
        public DateTime ActivityExecutionTime { get; set; }
        public DateTime Time { get; set; } // Ensure this is DateTime
        public string EntityResponsibleActivity { get; set; }
        public string ImagePath { get; set; } // Add if necessary
        public int ConcilMemberID { get; set; }

        [ForeignKey("ConcilMemberID")]
        public studentConcilMember ConcilMember { get; set; }

        public EventEntity() { }

        public EventEntity(int activityID, string activityName, string locationOfActivity, DateTime activityExecutionTime, DateTime time, string entityResponsibleActivity, int concilMemberID, string imagePath)
        {
            ActivityID = activityID;
            ActivityName = activityName;
            LocationOfActivity = locationOfActivity;
            ActivityExecutionTime = activityExecutionTime;
            this.Time = time;
            EntityResponsibleActivity = entityResponsibleActivity;
            ConcilMemberID = concilMemberID;
            ImagePath = imagePath;
        }
    }
}
