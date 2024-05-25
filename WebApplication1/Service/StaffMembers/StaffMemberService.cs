using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
using WebApplication1.Models;

namespace WebApplication1.Service.StaffMembers;

public interface IStaffMemberService
{
    Task<Table.Teacher> AddStaffMember(TeacherEntitycs m);
    Task<Table.Teacher> GetStaffMember(int id);
    void setsessionvalue(Table.Teacher teacher);
    Task <int> GetStaffMemberId(string name);
}

public class StaffMemberService : IStaffMemberService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public StaffMemberService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<Table.Teacher> AddStaffMember(TeacherEntitycs m)
    {
        var password = BCrypt.Net.BCrypt.HashPassword(m.password);
        var staffMember = new Table.Teacher(m.name, m.Id, m.email, password, m.confpassword, m.phone);
        
        _context.Teachers.Add(staffMember);
        await _context.SaveChangesAsync();
        return staffMember;

    }

    public async Task<Table.Teacher> GetStaffMember(int id)
    {
        var staffMember = await _context.Teachers.FirstOrDefaultAsync(x => x.TeacherID == id);
        return staffMember;
    }
    public async Task<int> GetStaffMemberId(string name)
    {
        var staffMember = await _context.Teachers.FirstOrDefaultAsync(x => x.Teachername.Equals(name));
        int id = staffMember.TeacherID;
        return staffMember.TeacherID;
    }
    public void setsessionvalue(Table.Teacher teacher)
    {
        _httpContextAccessor.HttpContext.Session.SetInt32("Id", teacher.TeacherID);
        _httpContextAccessor.HttpContext.Session.SetString("Name", teacher.Teachername);
        _httpContextAccessor.HttpContext.Session.SetString("Email", teacher.Email);
        _httpContextAccessor.HttpContext.Session.SetInt32("Phone", teacher.Phone);
        _httpContextAccessor.HttpContext.Session.SetString("Role", "teacher");
    }
   
}