using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Models;
using WebApplication1.Resorces;
using WebApplication1.Service.Probosal;
using WebApplication1.Services.Event;
using WebApplication1.Table;
namespace WebApplication1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProbosalController : Controller
    {
        private readonly IProbosalService _probosalService;

        public ProbosalController(IProbosalService probosalService)
        {
            _probosalService = probosalService;
        }
        [HttpPost("AddProbosal")]
        public  async Task<IActionResult> AddPRobosal(ProbosalEntity m)
        {
            var res=await _probosalService.AddProbosal(m);
            var resource = new ProbosalResource
            {
                probosalID = res.ProbosalID,
                probosalDescription = res.ProbosalDescribtion,
                NameOFCommittee = res.TargetParty,
                imagePath = res.ImagePath,
            };
            return Ok(resource);
        }
        [HttpPut("UpdateProbosal")]
        public async Task<IActionResult> UpdateProbosal(ProbosalEntity m,int id)
        {
            var existingProbosal=await _probosalService.UpdateProbosal(m,id);
            if (existingProbosal!=null)
            {
                var res = new ProbosalResource
                {
                    probosalID = existingProbosal.ProbosalID,
                    probosalDescription = existingProbosal.ProbosalDescribtion,
                    NameOFCommittee = existingProbosal.TargetParty,
                    imagePath = existingProbosal.ImagePath,
                };
                return Ok(res);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpDelete("DeleteProbosal")]
        public async Task<IActionResult> DeleteProbosal(int id)
        {
            var isDelete = await _probosalService.DeletePRobosal(id);
            if (!isDelete)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }



    }
}
