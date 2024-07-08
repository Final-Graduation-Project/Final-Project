using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Table;


namespace WebApplication1.Service.Student;

public interface IStudentService
{
    Task<Table.Student> AddStudent(studentEntity m);
    Task<Table.Student> GetStudent(int id);
    void setsessionvalue(Table.Student student);
    public string? GetCurrentLoggedIn();
    public void logout();
    public Task<string> changepassword(int id , string oldpassword,string newpassword);
    public Task<string> forgetpassword(int id, string password);
}
public class StudentService : IStudentService
{

    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

   
    public StudentService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
   
    
    
    public async Task<Table.Student> AddStudent(studentEntity m)
    {
        var password = BCrypt.Net.BCrypt.HashPassword(m.password);
       var student = new Table.Student(m.name, m.Id, m.email, password, m.universityMajor, m.phone,m.LastSeen);
        _context.Students.Add(student);


        await _context.SaveChangesAsync();

        return student;
    }
    public async Task<Table.Student> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        
        return student;
    }
    public void setsessionvalue(Table.Student student)
    {
        
        _httpContextAccessor.HttpContext.Session.SetInt32("Id", student.StudentID);
        _httpContextAccessor.HttpContext.Session.SetString("Name", student.StudentName);
        _httpContextAccessor.HttpContext.Session.SetString("Email", student.Email);
        _httpContextAccessor.HttpContext.Session.SetInt32("Phone", student.Phone);
        _httpContextAccessor.HttpContext.Session.SetString("Role", "student");
       
    }
    public string? GetCurrentLoggedIn()
    {
        return _httpContextAccessor.HttpContext.Session.GetString("Role");
    }
    public void logout()
    {
        _httpContextAccessor.HttpContext.Session.Clear();
    }

    public async Task<string> changepassword(int id, string oldpassword, string newpassword)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return "Student not found";
        }

        if (BCrypt.Net.BCrypt.Verify(oldpassword, student.Password))
        {
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newpassword);
            student.Password = newPasswordHash;
            await _context.SaveChangesAsync();
            return "Password Changed Successfully";
        }
        else
        {
            return "Old password is incorrect";
        }
    }

    public async Task<string> forgetpassword(int id, string password)
    {
        var student = await _context.Students.FindAsync(id);
        if (student==null)
        {
            return "student not found";
        }
        string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        student.Password = newPasswordHash;
        await _context.SaveChangesAsync();
        return "Password Changed Successfully";
    }
}