using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Models;
using WebApplication1.Resorces;
using WebApplication1.Service.concilMember;
using WebApplication1.Services.Event;

namespace WebApplication1.Controllers
{
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
            var res = await _IconcilMemberService.AddConcilmember(m);
            var resource = new concilMemberResource
            {
               concilID=res.ConcilMemberID,
               ConcilName=res.ConcilMemberName,
               email=res.Email,
               ResponsibleActivity=res.EntityResponsibleActivity,
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


                };
                return Ok(res);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
