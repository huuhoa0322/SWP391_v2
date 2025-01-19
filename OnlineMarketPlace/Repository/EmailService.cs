
using System.Net.Mail;
using System.Net;

namespace OnlineMarketPlace.Repository
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bat bao mat
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("fpthemath@gmail.com", "sparbgczfoxcfqoi")
            };

            return client.SendMailAsync(
            new MailMessage(from: "fpthemath@gmail.com",
            to: email,
            subject,
            message));
        }
    }
}
