using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class TeacherEntitycs
    {
        [Range(1000, 9999, ErrorMessage = "The Id must be a 7-digit number.")]
        public int Id { get; set; }
        [EmailAddress(ErrorMessage = "The email must be a valid email address.")]
        public string email { get; set; }
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z]).{8,}$", ErrorMessage = "The password must be at least 8 characters long and contain at least one character.")]
        public string password { get; set; }
        public string confpassword { get; set; }
        public string name { get; set; }
        public int phone { get; set; }
        public string universityMajor { get; set; }
        public DateTime LastSeen { get; set; } // New property


    }
}
