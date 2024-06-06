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
        public string ActivityExecutionTime { get; set; }
        public string time { get; set; }
        public string EntityResponsibleActivity { get; set; }
     //   public string ActivityDescription { get; set; }
      //  public int NumberParticipateActivity { get; set; }
        public string ImagePath { get; set; }

        public int ConcilMemberID { get; set; }

        [ForeignKey("ConcilMemberID")]
        public studentConcilMember ConcilMember { get; set; }

        public EventEntity() { }

        public EventEntity(int activityID, string activityName, string locationOfActivity, string activityExecutionTime, string time, string entityResponsibleActivity, int concilMemberID, string imagePath)
        {
            ActivityID = activityID;
            ActivityName = activityName;
            LocationOfActivity = locationOfActivity;
            ActivityExecutionTime = activityExecutionTime;
            this.time = time;
            EntityResponsibleActivity = entityResponsibleActivity;
            ConcilMemberID = concilMemberID;
            ImagePath = imagePath; 
        }
    }
}
