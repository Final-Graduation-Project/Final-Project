using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class studentEntity
    {
        [Range(1000000, 9999999, ErrorMessage = "The Id must be a 7-digit number.")]
        public int Id { get; set; }
        [EmailWithSpecificFormat("student.birzeit.edu", ErrorMessage = "Email must be in the format <7-digit-number>@student.birzite.edu.")]
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
