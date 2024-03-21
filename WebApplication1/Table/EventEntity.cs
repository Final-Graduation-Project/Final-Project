using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Table;

public class EventEntity
{
    [Key]
    public int ActivityID { get; set; }

    public string? ActivityName { get; set; }
    public string? LocationOfActivity { get; set; }
    public DateTime ActivityExecutionTime { get; set; } // Directly use DateTime instead of Func<DateTime>
    public DateTime DateImplementationActivity { get; set; } // Directly use DateTime instead of Func<DateTime>
    public string? EntityResponsibleActivity { get; set; }
    public string? ActivityDescription { get; set; }
    public int NumberParticipateActivity { get; set; }

    [ForeignKey("Student")]
    public int StudentID { get; set; }
    
    public Student? Student { get; set; }

    public EventEntity(int activityID, string activityName, string locationOfActivity, DateTime activityExecutionTime, DateTime dateImplementationActivity, string entityResponsibleActivity, string activityDescription, int numberParticipateActivity, int studentID)
    {
        ActivityID = activityID;
        ActivityName = activityName;
        LocationOfActivity = locationOfActivity;
        ActivityExecutionTime = activityExecutionTime;
        DateImplementationActivity = dateImplementationActivity;
        EntityResponsibleActivity = entityResponsibleActivity;
        ActivityDescription = activityDescription;
        NumberParticipateActivity = numberParticipateActivity;
        StudentID = studentID;
    }
}
