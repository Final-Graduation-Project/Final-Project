using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Resorces;
using WebApplication1.Service.concilMember;

[Route("api/[controller]")]
[ApiController]
public class concilMemberController : Controller
{
    private readonly IconcilMemberService _IconcilMemberService;

    public concilMemberController(IconcilMemberService iconcilMemberService)
    {
        _IconcilMemberService = iconcilMemberService;
    }

    [HttpPost("AddConcilmember")]
    public async Task<IActionResult> AddConcilmember(concilMemberEntity m)
    {
        if (await _IconcilMemberService.GetConcilMemberById(m.concilMemberID) != null)
            return BadRequest("Student Already Exist");
        if (m.password != m.confirm_password)
            return BadRequest("Password Not Matched");
        var res = await _IconcilMemberService.AddConcilmember(m);
        var resource = new concilMemberResource
        {
            concilID = res.ConcilMemberID,
            ConcilName = res.ConcilMemberName,
            email = res.Email,
            ResponsibleActivity = res.EntityResponsibleActivity,
            LastSeen = res.LastSeen
        };
        return Ok(resource);
    }

    [HttpPut("UpdateConcilmember")]
    public async Task<IActionResult> UpdateConcilmember([FromBody] concilMemberEntity updateConcil)
    {
        var updateconcil = await _IconcilMemberService.UpdateConcilmember(updateConcil);
        if (updateconcil != null)
        {
            var res = new concilMemberResource
            {
                concilID = updateconcil.ConcilMemberID,
                ConcilName = updateconcil.ConcilMemberName,
                email = updateconcil.Email,
                ResponsibleActivity = updateconcil.EntityResponsibleActivity,
                LastSeen = updateconcil.LastSeen
            };
            return Ok(res);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet("GetConcilMemberById/{id}")]
    public async Task<IActionResult> GetConcilMemberById(int id)
    {
        var concilMember = await _IconcilMemberService.GetConcilMemberById(id);
        if (concilMember == null)
        {
            return NotFound();
        }

        var resource = new concilMemberResource
        {
            concilID = concilMember.ConcilMemberID,
            ConcilName = concilMember.ConcilMemberName,
            email = concilMember.Email,
            ResponsibleActivity = concilMember.EntityResponsibleActivity,
            LastSeen = concilMember.LastSeen
        };

        return Ok(resource);
    }

    [HttpGet("GetAllConcilMembers")]
    public async Task<IActionResult> GetAllConcilMembers()
    {
        var concilMembers = await _IconcilMemberService.GetAllConcilMembers();
        var resources = concilMembers.Select(cm => new concilMemberResource
        {
            concilID = cm.ConcilMemberID,
            ConcilName = cm.ConcilMemberName,
            email = cm.Email,
            ResponsibleActivity = cm.EntityResponsibleActivity,
            LastSeen = cm.LastSeen
        });

        return Ok(resources);
    }
    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword(int id, string oldPassword, string newPassword)
    {
        string change = await _IconcilMemberService.changepassword(id, oldPassword, newPassword);
        if (change == "Password Changed Successfully")
        {
            return Ok(change);
        }
        else
        {
            return BadRequest(change);
        }
    }

}
