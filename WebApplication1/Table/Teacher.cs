using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Table;

public class Teacher
{
    private string _teachername;
    private int _teacherID;
    private string _email;
    private string _password;
    private string _confirmPassword;
    private int _phone;

    // Navigation property for related entities
    public ICollection<OfficeHour> OfficeHours { get; set; }

    public Teacher(string teachername, int teacherID, string email, string password, string confirmPassword, int phone)
    {
        _teachername = teachername;
        _teacherID = teacherID;
        _email = email;
        _password = password;
        _confirmPassword = confirmPassword;
        _phone = phone;

        // Initialize OfficeHours collection
        OfficeHours = new List<OfficeHour>();
    }

    [Required]
    public string Teachername
    {
        get { return _teachername; }
        set { _teachername = value; }
    }

    [Key]
    public int TeacherID
    {
        get { return _teacherID; }
        set { _teacherID = value; }
    }

    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }

    public string Password
    {
        get { return _password; }
        set { _password = value; }
    }

    public string ConfirmPassword
    {
        get { return _confirmPassword; }
        set { _confirmPassword = value; }
    }

    public int Phone
    {
        get { return _phone; }
        set { _phone = value; }
    }
}