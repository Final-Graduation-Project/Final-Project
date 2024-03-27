using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Table
{
    public class studentConcilMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConcilMemberID { get; set; }

        public string ConcilMemberName { get; set; }

        public string Email { get; set; }

        public string EntityResponsibleActivity { get; set; }

        public ICollection<EventEntity> Events { get; set; }

        public studentConcilMember()
        {
            Events = new List<EventEntity>();
        }

        public studentConcilMember(int concilID, string concilName, string email, string responsibleActivity)
        {
            ConcilMemberID = concilID;
            ConcilMemberName = concilName;
            Email = email;
            EntityResponsibleActivity = responsibleActivity;

        }
    }
}
