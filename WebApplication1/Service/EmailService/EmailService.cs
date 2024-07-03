using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace WebApplication1.Service.EmailService
{
    public interface IEmailService
    {
        Task<IActionResult> CreateAndSendCampaign(string email, string token);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly string _apiKey = "368af3bc27a25c5c2813f41b73409c79-623e10c8-7acced07";
        private readonly string _domain = "sandboxed545f10c1b348cd9fe918483e35aac3.mailgun.org";

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> CreateAndSendCampaign(string email, string token)
        {
            var client = new RestClient($"https://api.mailgun.net/v3/{_domain}");
            client.AddDefaultHeader("Authorization", $"Basic {Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("api:" + _apiKey))}");

            try
            {
                // Create campaign content
                var emailFrom = "z.j.masalma@gmail.com";
                var emailFromName = "Your Name";
                var emailSubject = "Subject of the Campaign";
                var emailBodyHtml = $"<p>Body of the Campaign with token: {token}</p>";
                var emailBodyText = $"Body of the Campaign with token: {token}";

                // Send the email
                var request = new RestRequest("messages", Method.Post);
                request.AddParameter("from", $"{emailFromName} <{emailFrom}>");
                request.AddParameter("to", email);
                request.AddParameter("subject", emailSubject);
                request.AddParameter("text", emailBodyText);
                request.AddParameter("html", emailBodyHtml);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    _logger.LogInformation($"Campaign sent successfully. StatusCode: {response.StatusCode}");
                    return new OkResult();
                }
                else
                {
                    _logger.LogError($"Error sending campaign. StatusCode: {response.StatusCode}, Error: {response.Content}");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating and sending campaign");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
