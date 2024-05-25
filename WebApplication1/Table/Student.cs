using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Table;

public class Student
{
     private string studentName;
        private int studentID;
        private string email;
        private string password;
        private string universityMajor;
        private int phone;
    [NotMapped]
    public List<Message> SentMessages { get; set; }

    [NotMapped]
    public List<Message> ReceivedMessages { get; set; }





    public Student()
    {

    }
    
        public Student(string studentName, int studentID, string email, string password , string universityMajor, int phone)
        {
            this.studentName = studentName;
            this.studentID = studentID;
            this.email = email;
            this.password = password;
            this.universityMajor = universityMajor;
            this.phone = phone;
        }
    
        [Required]
        public string StudentName 
        { 
            get { return studentName; } 
            set { studentName = value; } 
        }
    
        [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int StudentID 
        { 
            get { return studentID; } 
            set { studentID = value; } 
        }
    
        [Required]
        public string Email 
        { 
            get { return email; } 
            set { email = value; } 
        }
    
        [Required]
        public string Password 
        { 
            get { return password; } 
            set { password = value; } 
        }
    
       
        [Required]
        public string UniversityMajor 
        { 
            get { return universityMajor; } 
            set { universityMajor = value; } 
        }
    
        [Required]
        public int Phone 
        { 
            get { return phone; } 
            set { phone = value; } 
        }

    

}