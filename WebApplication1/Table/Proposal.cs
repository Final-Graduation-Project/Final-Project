using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Proposal
    {
        [Key]
        public int ProposalID { get; set; }
        public string Type { get; set; }
        public string Question { get; set; }
        public string Committee { get; set; }
        public int Votes { get; set; }
        public bool Accepted { get; set; }
        public int UserID { get; set; }
        public string OptionText { get; set; }
        public string CommentText { get; set; }
        public string VotedUsers { get; set; } // إضافة الحقل الجديد
    
        public string name { get; set; }
    }

}
