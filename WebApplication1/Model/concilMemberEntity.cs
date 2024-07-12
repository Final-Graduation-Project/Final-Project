using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Table;

namespace WebApplication1.Model
{
    public class concilMemberEntity
    {
        [Range (1000000, 9000000, ErrorMessage ="The ID must be a 7-digit number.")]
        public int concilMemberID { get; set; }

        public string password {  get; set; }

        public string confirm_password { get; set; }

        public string concilMemberName { get; set; }

        [EmailWithSpecificFormat("student.birzeit.edu", ErrorMessage = "Email must be in the format <7-digit-number>@student.birzite.edu.")]
        public string email { get; set; }

        public string EntityResponsibleActivity { get; set; }
        public DateTime LastSeen { get; set; } // New property



    }
}


public class EmailWithSpecificFormatAttribute : ValidationAttribute
{
    private readonly string _allowedDomain;

    public EmailWithSpecificFormatAttribute(string allowedDomain)
    {
        _allowedDomain = allowedDomain;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            string email = value.ToString();
            if (email.EndsWith($"@{_allowedDomain}", StringComparison.OrdinalIgnoreCase))
            {
                string[] parts = email.Split('@');
                if (parts.Length == 2 && parts[0].Length == 7 && int.TryParse(parts[0], out _))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult($"Email must be in the format <7-digit-number>@{_allowedDomain}.");
        }
        return ValidationResult.Success; // Returning success if the value is null
    }
}

