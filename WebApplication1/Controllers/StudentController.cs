using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;
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
    public StudentController(IStudentService studentService, IStaffMemberService staffMemberService)
    {
        _studentService = studentService;
        _staffMemberService = staffMemberService;
    }
   
    [HttpPost("AddStudent")]
    public async Task<IActionResult> AddStudent(Model.RigModl m)
    {
        /*if(_studentService.GetStudent(m.Id) != null)
            return BadRequest("Student Already Exist");*/
        if (m.password != m.confpassword)
            return BadRequest("Password Not Matched");
        
        var res =await _studentService.AddStudent(m);
        return Ok(res);
    }
    [HttpGet("GetStudent")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var res =await _studentService.GetStudent(id);
        return Ok(res);
    }
    [HttpPost("login")]
    public async Task<IActionResult> login(Model.LoginModel u)
    {
        if (Math.Abs(u.Id) == 7)
        {
            Student user = await _studentService.GetStudent(u.Id);
            if (user == null)
            {
                return BadRequest("user dose not exist");
            }
            if (u.password!= user.Password)
            {
                return BadRequest("wrong password");
            }
            _studentService.setsessionvalue(user);
            return Ok("Login Success");
        }
        else if (Math.Abs(u.Id) == 4)
        {
            Teacher user = await _staffMemberService.GetStaffMember(u.Id);
            if (user == null)
            {
                return BadRequest("user dose not exist");
            }
            if (u.password!= user.Password)
            {
                return BadRequest("wrong password");
            }
            _staffMemberService.setsessionvalue(user);
            return Ok("Login Success");
        }

        return BadRequest("Invalid Id");

    }
    [HttpGet]
    public IActionResult Get()
    {
        int? id=_studentService.GetCurrentLoggedIn();
        return Ok(id);
    }
    [HttpGet("logout")]
    public IActionResult logout()
    {
        _studentService.logout();
        return Ok("Logout Success");
    }
   
 
}