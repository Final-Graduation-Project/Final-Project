using Microsoft.AspNetCore.Mvc;
using WebApplication1.Service.StaffMembers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebApplication1.Resorces;
using WebApplication1.Service.EmailConfirmation;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IStaffMemberService _staffMemberService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
    IStaffMemberService staffMemberService,
    IEmailSender emailSender,
    ILogger<AccountController> logger)
        {
            _staffMemberService = staffMemberService;
            _emailSender = emailSender;
            _logger = logger;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(TeacherEntitycs model)
        {
            if (ModelState.IsValid)
            {
                // Check if the provided password matches the confirmed password
                if (model.password != model.confpassword)
                {
                    ModelState.AddModelError(string.Empty, "The password and confirmation password do not match.");
                    return BadRequest(ModelState);
                }

                // Create a new user entity based on the provided model
                var user = new TeacherEntitycs
                {
                    name = model.name,
                    email = model.email,
                    password = model.password,
                    confpassword = model.confpassword,
                    phone = model.phone,
                    universityMajor = model.universityMajor
                };

                // Call your service to add the user to the database
                var result = await _staffMemberService.AddStaffMember(user);

                if (result != null)
                {
                    // If user creation is successful, generate the email confirmation token
                    var token = GenerateEmailConfirmationToken();
                    await SendConfirmationEmail(user.email, token);

                    // Optionally, you may redirect the user to a page indicating successful registration
                    return RedirectToAction("RegistrationConfirmation");
                }
                else
                {
                    // If user creation fails, add errors to the ModelState
                    ModelState.AddModelError(string.Empty, "Failed to register user.");
                    return BadRequest(ModelState);
                }
            }

            // If the ModelState is invalid, return the model with errors
            return BadRequest(ModelState);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            // Decode the token if needed
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // Verify email with token (Implement your own logic here)
            var isEmailConfirmed = VerifyEmailWithToken(email, token);

            if (isEmailConfirmed)
            {
                // Optionally, you may redirect the user to a page indicating successful email confirmation
                return RedirectToAction("EmailConfirmation");
            }
            else
            {
                // If email confirmation fails, return an error message
                return BadRequest("Email confirmation failed.");
            }
        }

        // Implement your own logic to generate email confirmation token
        private string GenerateEmailConfirmationToken()
        {
            // Generate a random token (Implement your own logic here)
            return "RandomToken";
        }

        // Implement your own logic to send confirmation email
        private async Task SendConfirmationEmail(string email, string token)
        {
            // Send confirmation email (Implement your own logic here)
            await _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your email by clicking this link: {token}");
        }

        // Implement your own logic to verify email with token
        private bool VerifyEmailWithToken(string email, string token)
        {
            // Verify email with token (Implement your own logic here)
            return true;
        }
    }
}
