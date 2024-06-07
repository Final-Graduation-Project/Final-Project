using Microsoft.AspNetCore.Http.HttpResults;
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
            email=res.Email,
            LastSeen = res.LastSeen
        };

        return Ok(resorce);
    }

    [HttpGet("GetStaffMember/{id}")]
    public async Task<IActionResult> GetStaffMember(int id)
    {
        var teacer = await _staffMemberService.GetStaffMember(id);
        if (teacer == null)
        {
            return NotFound();
        }
        var resource = new StaffMemberResource
        {
            ID = teacer.TeacherID,
            name = teacer.Teachername,
            email = teacer.Email
        };
        return Ok(resource);
    }
    [HttpGet("GetAllStaffMember")]
    public async Task<IActionResult> GetAllStaffMember()
    {
        var tescher= await _staffMemberService.GetAllStaffMember();
        
        return Ok(tescher);
    }



}