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
        public DateTime DateImplementationActivity { get; set; }
        public string EntityResponsibleActivity { get; set; }
        public string ActivityDescription { get; set; }
        public int NumberParticipateActivity { get; set; }
        public int ConcilMemberID { get; set; }

        [ForeignKey("ConcilMemberID")]
        public studentConcilMember ConcilMember { get; set; }

        public EventEntity() { }

        public EventEntity(int activityID, string activityName, string locationOfActivity, DateTime activityExecutionTime, DateTime dateImplementationActivity, string entityResponsibleActivity, string activityDescription, int numberParticipateActivity, int concilMemberID)
        {
            ActivityID = activityID;
            ActivityName = activityName;
            LocationOfActivity = locationOfActivity;
            ActivityExecutionTime = activityExecutionTime;
            DateImplementationActivity = dateImplementationActivity;
            EntityResponsibleActivity = entityResponsibleActivity;
            ActivityDescription = activityDescription;
            NumberParticipateActivity = numberParticipateActivity;
            ConcilMemberID = concilMemberID;
        }
    }
}
