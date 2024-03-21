using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Table;

public class Event
{
    [Key]
    public int ActivityID { get; set; }

    public string? ActivityName { get; set; }
    public string? LocationOfActivity { get; set; }
    public DateTime ActivityExecutionTime { get; set; } 
    public DateTime DateImplementationActivity { get; set; }
    public string? EntityResponsibleActivity { get; set; }
    public string? ActivityDescription { get; set; }
    public int NumberParticipateActivity { get; set; }

    [ForeignKey("Student")]
    public int StudentID { get; set; }
    
    public Student? Student { get; set; }
}