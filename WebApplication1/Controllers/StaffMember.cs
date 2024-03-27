using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Models;
using WebApplication1.Resorces;
using WebApplication1.Service.StaffMembers;

namespace WebApplication1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StaffMember : Controller
{
    private readonly IStaffMemberService _staffMemberService;
    public StaffMember(IStaffMemberService staffMemberService)
    {
        _staffMemberService = staffMemberService;
    }
    [HttpPost("AddStaffMember")]
    public async Task<IActionResult> AddStaffMember(TeacherEntitycs m)
    {
        if (m.password != m.confpassword)
            return BadRequest("Password Not Matched");
        
        var res =await _staffMemberService.AddStaffMember(m);

        var resorce = new StaffMemberResource
        {
            ID=res.TeacherID,
            name = res.Teachername,
            email=res.Email
        };

        return Ok(resorce);
    }


   
}