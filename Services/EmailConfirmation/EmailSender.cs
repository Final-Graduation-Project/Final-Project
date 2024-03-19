using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Service.EmailConfirmation;



namespace WebApplication1.Service.EmailConfirmation
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = _emailSettings.MailServer;
                smtpClient.Port = _emailSettings.MailPort;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                    mailMessage.Subject = subject;
                    mailMessage.Body = htmlMessage;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(email);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
