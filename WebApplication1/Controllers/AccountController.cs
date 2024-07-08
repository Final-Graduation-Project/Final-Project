using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using WebApplication1.Model;
using WebApplication1.Models;
using WebApplication1.Service.EmailService;
using WebApplication1.Service.StaffMembers;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IEmailService _emailService;

        public AccountController(
            IEmailService emailService
            )
        {
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var em = await _emailService.CreateAndSendCampaign(email, token);
            return Ok(em);
        }



    }
}
