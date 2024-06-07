using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Service.concilMember;
using WebApplication1.Service.StaffMembers;
using WebApplication1.Service.Student;
using WebApplication1.Table;

namespace WebApplication1.Controllers;
[Route("api/[controller]")]
[ApiController]

public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IStaffMemberService _staffMemberService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IconcilMemberService _concilMemberService;
    public StudentController(IStudentService studentService, IStaffMemberService staffMemberService, IHttpContextAccessor httpContextAccessor, IconcilMemberService concilMemberService)
    {
        _studentService = studentService;
        _staffMemberService = staffMemberService;
        _httpContextAccessor = httpContextAccessor;
        _concilMemberService = concilMemberService;
    }
   
    [HttpPost("AddStudent")]
    
    public async Task<IActionResult> AddStudent(studentEntity m)
    {
        if(await _studentService.GetStudent(m.Id) != null)
            return BadRequest("Student Already Exist");
        if (m.password != m.confpassword)
            return BadRequest("Password Not Matched");

        var res =await _studentService.AddStudent(m);
        return Ok(res);
    }
    [HttpGet("GetStudent/{id}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var res =await _studentService.GetStudent(id);
        return Ok(res);
    }
    [HttpPost("login")]
    public async Task<IActionResult> login(LoginEntity u)
    {
        if (u.Id.ToString().Length==7)
        {
            Student user = await _studentService.GetStudent(u.Id);
            if (user == null)
            {
                studentConcilMember concilMember = await _concilMemberService.GetConcilMemberById(u.Id);
                if (concilMember == null)
                {
                    return BadRequest("user dose not exist");
                }
                if (u.password != concilMember.password)
                {
                    return BadRequest("wrong password");
                }
                _concilMemberService.setsessionvalue(concilMember);
            }
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(u.password, user.Password))
                {
                    return BadRequest("wrong password");
                }

                _studentService.setsessionvalue(user);
            }

            //in userdetails set all details in session
            Dictionary<string, string> sessionValues = new Dictionary<string, string>();

            // Retrieve session values
            int? id = _httpContextAccessor.HttpContext.Session.GetInt32("Id");
            string name = _httpContextAccessor.HttpContext.Session.GetString("Name");
            string email = _httpContextAccessor.HttpContext.Session.GetString("Email");
            int? phone = _httpContextAccessor.HttpContext.Session.GetInt32("Phone");
            string role = _httpContextAccessor.HttpContext.Session.GetString("Role");

            // Add session values to dictionary
            sessionValues["Id"] = id?.ToString() ?? string.Empty;
            sessionValues["Name"] = name ?? string.Empty;
            sessionValues["Email"] = email ?? string.Empty;
            sessionValues["Phone"] = phone?.ToString() ?? string.Empty;
            sessionValues["Role"] = role ?? string.Empty;

            // Serialize dictionary to JSON
            string json = JsonConvert.SerializeObject(sessionValues);
            return Ok(json);
        }
        else if (u.Id.ToString().Length==4)
        {
            Teacher user = await _staffMemberService.GetStaffMember(u.Id);
            if (user == null)
            {
                return BadRequest("user dose not exist");
            }
            if (!BCrypt.Net.BCrypt.Verify(u.password, user.Password))
            {
                return BadRequest("wrong password");
            }
            _staffMemberService.setsessionvalue(user);
            //in userdetails set all details in session
            Dictionary<string, string> sessionValues = new Dictionary<string, string>();

            // Retrieve session values
            int? id = _httpContextAccessor.HttpContext.Session.GetInt32("Id");
            string name = _httpContextAccessor.HttpContext.Session.GetString("Name");
            string email = _httpContextAccessor.HttpContext.Session.GetString("Email");
            int? phone = _httpContextAccessor.HttpContext.Session.GetInt32("Phone");
            string role = _httpContextAccessor.HttpContext.Session.GetString("Role");

            // Add session values to dictionary
            sessionValues["Id"] = id?.ToString() ?? string.Empty;
            sessionValues["Name"] = name ?? string.Empty;
            sessionValues["Email"] = email ?? string.Empty;
            sessionValues["Phone"] = phone?.ToString() ?? string.Empty;
            sessionValues["Role"] = role ?? string.Empty;

            // Serialize dictionary to JSON
            string json = JsonConvert.SerializeObject(sessionValues);
            return Ok(json);
        }

        return BadRequest("Invalid Id");

    }
    [HttpGet]
    public IActionResult Get()
    {
        string? role=_studentService.GetCurrentLoggedIn();
        return Ok(role);
    }
    [HttpGet("logout")]
    public IActionResult logout()
    {
        _studentService.logout();
        return Ok("Logout Success");
    }
   
 
}