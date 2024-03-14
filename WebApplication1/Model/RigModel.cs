using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model;

public class RigModl
{
    [Range(1000000, 9999999, ErrorMessage = "The Id must be a 7-digit number.")]
    public int Id { get; set; }
    [EmailAddress (ErrorMessage = "The email must be a valid email address.")]
    public String email { get; set; }
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Za-z]).{8,}$", ErrorMessage = "The password must be at least 8 characters long and contain at least one character.")]
    public String password { get; set; }
    public String confpassword { get; set; }
    public String name { get; set; }
    public int phone { get; set; }
    public String universityMajor { get; set; }
}